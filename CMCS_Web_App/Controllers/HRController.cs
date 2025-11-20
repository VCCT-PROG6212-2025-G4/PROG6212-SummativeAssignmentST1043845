using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CMCS_Web_App.Controllers
{

    public class HRController : Controller
    {
        private readonly AppDbContext _context;

        public HRController(AppDbContext context)
        {
            _context = context;
        }
        //  Role protection using Session (NO Identity)
        [HttpGet("HR/HRDash")]
        public IActionResult HRDash()
        {
            Console.WriteLine("HRDash method triggered");

            // Session role validation
            if (HttpContext.Session.GetString("Role") != "HR")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            // Optional: HR Name display
            ViewBag.HRName = HttpContext.Session.GetString("UserName");

            return View("HRDash");
        }

        private bool IsHR()
        {
            return HttpContext.Session.GetString("Role") == "HR";
        }


        // =============================
        // 1. VIEW ALL LECTURERS
        // =============================
        public IActionResult LecturersManager()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Home");

            var lecturers = _context.Lecturers.ToList();
            return View(lecturers);
        }

        // =============================
        // 2. EDIT HOURLY RATE (GET)
        // =============================
        public IActionResult Rate(int id)
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Home");

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();

            return View(lecturer);
        }

        // =============================
        // 3. EDIT HOURLY RATE (POST)
        // =============================
        [HttpPost]
        public IActionResult Rate(int id, decimal ratePerHour)
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Home");

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();

            lecturer.RatePerHour = ratePerHour;

            _context.SaveChanges();

            TempData["Success"] = "Hourly rate updated successfully.";
            return RedirectToAction("ManageLecturers");
        }

        // =============================
        // 4. VIEW APPROVED CLAIMS
        // =============================
        public IActionResult ApprovedClaims()
        {
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Home");

            var claims = _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .OrderByDescending(c => c.DateApproved)
                .ToList();

            return View(claims);
        }
    }
}
    

