using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CMCS_Web_App.Data.AppDbContext;


namespace CMCS_Web_App.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDbContext _context;

        public HRController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsHR()
        {
            return HttpContext.Session.GetString("Role") == "HR";
        }

        // ======================================
        // VIEW ALL LECTURERS
        // ======================================
        public IActionResult LecturersManager()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            var lecturers = _context.Lecturers.ToList();
            return View(lecturers);
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

        // ======================================
        // UPDATE HOURLY RATE (within lecturer list)
        // ======================================
        [HttpPost]
        public async Task<IActionResult> UpdateHourlyRate(int lecturerId, decimal rate)
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            var lecturer = await _context.Lecturers.FindAsync(lecturerId);
            if (lecturer == null)
                return NotFound();

            lecturer.RatePerHour = rate;

            _context.Update(lecturer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hourly rate updated!";
            return RedirectToAction("LecturersManager");
        }

        // ======================================
        // CREATE A USER (HR creates all roles)
        // ======================================
        [HttpGet]
        public IActionResult CreateUser()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            if (!ModelState.IsValid)
                return View(user);

            // Save user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Auto-generate a matching Lecturer if needed
            if (user.Role == "Lecturer")
            {
                var lecturer = new Lecturer
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
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