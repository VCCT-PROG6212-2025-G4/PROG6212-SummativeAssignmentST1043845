using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using CMCS_Web_App.Services;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Xml.Linq;

using static CMCS_Web_App.Data.AppDbContext;

using XBrushes = PdfSharpCore.Drawing.XBrushes;
using XStringFormats = PdfSharpCore.Drawing.XStringFormats;

namespace CMCS_Web_App.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ReportService _reportService;
        public HRController(AppDbContext context, ReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        [HttpGet("HR/HRDash")]
        public IActionResult HRDash()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            // Not logged in
            if (userId == null || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Wrong role
            if (role != "HR")
            {
                return RedirectToAction("AccessDenied", "Auth");
            }

            // Optional: show name on UI
            ViewBag.HRName = HttpContext.Session.GetString("UserName");

            return View();
        }



        private bool IsHR()
        {
            return HttpContext.Session.GetString("Role") == "HR";
        }


        // ================================
        // USER MANAGER (NEW - replaces LecturerManager)
        // ================================
        public IActionResult UserManager()
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            // Get all users and lecturers
            var users = _context.Users.ToList();
            var lecturers = _context.Lecturers.ToList();

            // Build view model
            var model = users.Select(u =>
            {
                // Only try to find a lecturer for users who have the role "Lecturer"
                Lecturer lec = null;
                if (u.Role == "Lecturer")
                {
                    lec = lecturers.FirstOrDefault(x => x.Email == u.Email);
                }

                return new User
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,

                    LecturerId = lec?.LecturerId,
                    Department = lec?.Department,
                    RatePerHour = lec?.RatePerHour
                };
            }).ToList();

            return View(model);
        }


        // ======================================
        // VIEW APPROVED CLAIMS
        // ======================================
        public async Task<IActionResult> ApprovedClaims()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            var approvedClaims = await _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            return View(approvedClaims);
        }

        [HttpGet("HR/ApprovedClaimsReport")]
        public IActionResult ApprovedClaimsReport(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(c => c.DateSubmitted >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(c => c.DateSubmitted <= toDate.Value);

            var claims = query.ToList();

            var summary = claims
                .GroupBy(c => c.Lecturer.Email)
                .Select(g => new LecturerClaimsSummaryVM
                {
                    LecturerName = $"{g.First().Lecturer.FirstName} {g.First().Lecturer.LastName}",
                    TotalHoursWorked = g.Sum(x => x.HoursWorked),
                    RatePerHour = g.First().Lecturer.RatePerHour,
                    TotalAmountPaid = g.Sum(x => x.HoursWorked * x.Lecturer.RatePerHour)
                })
                .ToList();

            var vm = new ApprovedClaimsReportVM
            {
                Claims = claims,
                Summary = summary,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(vm);
        }




        // 2. Generate report (HTML view or PDF)
        [HttpPost]
        public FileResult GenerateApprovedClaimsReport(List<Claim> claimList, DateTime? dateSubmitted)
        {
            using var stream = new MemoryStream();
            var pdf = new PdfSharpCore.Pdf.PdfDocument();

            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
            var font = new XFont("Verdana", 10);

            gfx.DrawString("Approved Claims Report", titleFont, XBrushes.Black,
                new XRect(0, 20, page.Width, 20), XStringFormats.TopCenter);

            if (dateSubmitted.HasValue)
            {
                gfx.DrawString($"Filtered by Date Submitted: {dateSubmitted.Value:yyyy-MM-dd}",
                    font, XBrushes.Black,
                    new XRect(20, 50, page.Width - 40, 20), XStringFormats.TopLeft);
            }

            int yOffset = 90;

            foreach (var claim in claimList)
            {
                var lecturerName = claim.Lecturer != null
                    ? $"{claim.Lecturer.FirstName} {claim.Lecturer.LastName}"
                    : "Unknown Lecturer";

                var text = $"Claim #{claim.ClaimId} - R{claim.TotalAmount:N2} - Lecturer: {lecturerName} - Submitted: {claim.DateSubmitted:yyyy-MM-dd}";

                gfx.DrawString(text, font, XBrushes.Black,
                    new XRect(20, yOffset, page.Width - 40, 20), XStringFormats.TopLeft);

                yOffset += 20;

                if (yOffset > page.Height - 60)
                {
                    page = pdf.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yOffset = 40;
                }
            }

            pdf.Save(stream); // ✔ FIXED
            stream.Position = 0;

            return File(stream.ToArray(), "application/pdf", "ApprovedClaims.pdf");
        }


        // 3. Invoice generation UI (choose lecturer or all)
        [HttpGet]
        public IActionResult Invoices()
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            // pass lecturers list to choose from
            var lecturers = _context.Lecturers.ToList();
            return View(lecturers);
        }

        // 4. Generate invoice(s)
        [HttpPost]
        public async Task<IActionResult> GenerateInvoices(int? lecturerId, DateTime from, DateTime to, string output = "pdf")
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            // get approved claims grouped by lecturer
            var claims = await _reportService.GetApprovedClaimsAsync(from, to);
            var grouped = claims.GroupBy(c => c.Lecturer.Email);

            if (lecturerId.HasValue)
            {
                var singleLect = _context.Lecturers.Find(lecturerId.Value);
                if (singleLect == null) return NotFound();

                var theirClaims = claims.Where(c => c.Lecturer.LecturerId == lecturerId.Value).ToList();
                var html = _reportService.BuildInvoiceHtml($"{singleLect.FirstName} {singleLect.LastName}", singleLect.Email, theirClaims, from, to);

                if (output == "pdf")
                {
                    var pdf = _reportService.ConvertHtmlToPdf(html);
                    if (pdf == null) return Content(html, "text/html");
                    return File(pdf, "application/pdf", $"Invoice_{singleLect.LastName}_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
                }
                return Content(html, "text/html");
            }
            else
            {
                // Batch: create ZIP of PDFs (or return HTMLs)
                using var mem = new MemoryStream();
                using var archive = new System.IO.Compression.ZipArchive(mem, System.IO.Compression.ZipArchiveMode.Create, true);

                foreach (var grp in grouped)
                {
                    var lec = grp.First().Lecturer;
                    var html = _reportService.BuildInvoiceHtml($"{lec.FirstName} {lec.LastName}", lec.Email, grp, from, to);
                    if (output == "pdf")
                    {
                        var pdf = _reportService.ConvertHtmlToPdf(html) ?? System.Text.Encoding.UTF8.GetBytes(html);
                        var entry = archive.CreateEntry($"Invoice_{lec.LastName}_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
                        using var es = entry.Open();
                        es.Write(pdf, 0, pdf.Length);
                    }
                    else
                    {
                        var entry = archive.CreateEntry($"Invoice_{lec.LastName}_{from:yyyyMMdd}_{to:yyyyMMdd}.html");
                        using var es = new StreamWriter(entry.Open());
                        es.Write(html);
                    }
                }

                archive.Dispose();
                mem.Position = 0;
                var contentType = "application/zip";
                return File(mem.ToArray(), contentType, $"Invoices_{from:yyyyMMdd}_{to:yyyyMMdd}.zip");
            }
        }
     

        [HttpPost]
        public IActionResult AdjustRate(int lecturerId, int value)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(x => x.LecturerId == lecturerId);
            if (lecturer == null) return NotFound();

            lecturer.RatePerHour += value;
            _context.SaveChanges();

            return RedirectToAction("UserManager");
        }


        // ================================
        // EDIT USER (GET)
        // ================================

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(x => x.LecturerId == user.LecturerId);

            var vm = new EditUserVM
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,

                LecturerId = user.LecturerId,
                Department = lecturer?.Department,
                RatePerHour = lecturer?.RatePerHour
            };

            return View(vm);
        }



        // ================================
        // EDIT USER (POST)
        // ================================
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM vm)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var user = await _context.Users.FindAsync(vm.UserId);
            if (user == null) return NotFound();

            // Update User table
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.Role = vm.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // If role is Lecturer, update or create lecturer
            if (vm.Role == "Lecturer")
            {
                Lecturer lecturer;

                if (vm.LecturerId == null)
                {
                    // create new lecturer
                    lecturer = new Lecturer
                    {
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Email = vm.Email,
                        Department = vm.Department ?? "Unassigned",
                        RatePerHour = vm.RatePerHour ?? 0m
                    };

                    _context.Lecturers.Add(lecturer);
                }
                else
                {
                    lecturer = await _context.Lecturers.FindAsync(vm.LecturerId);

                    lecturer.FirstName = vm.FirstName;
                    lecturer.LastName = vm.LastName;
                    lecturer.Email = vm.Email;
                    lecturer.Department = vm.Department ?? lecturer.Department;
                    lecturer.RatePerHour = vm.RatePerHour ?? lecturer.RatePerHour;

                    _context.Lecturers.Update(lecturer);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserManager");
        }



        // ======================================
        // CREATE A USER (HR creates all roles)
        // ======================================
        [HttpGet]
        public IActionResult CreateUser()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            return View(new CreateUserVM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM vm)
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            if (!ModelState.IsValid)
                return View(vm);

            string hashedPassword = PasswordHasher.Hash(vm.Password);

            // Create User entity
            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                Role = vm.Role,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Auto-create Lecturer if needed
            if (vm.Role == "Lecturer")
            {
                var lecturer = new Lecturer
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = vm.Email,
                    Department = "Unassigned",
                    RatePerHour = 0
                };

                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "User created successfully!";
            return RedirectToAction("CreateUser");
        }
    }
}