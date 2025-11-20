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
        // =============================
        // Helper: Check if user is logged in and is HR
        // =============================
        private bool IsHR()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");

            return !string.IsNullOrEmpty(userId) && role == "HR";
        }

        // =============================
        // HR Dashboard
        // =============================
        [HttpGet("HR/HRDash")]
        public IActionResult HRDash()
        {
            Console.WriteLine("HRDash method triggered");

            if (!IsHR())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    return RedirectToAction("Login", "Auth");

                return RedirectToAction("AccessDenied", "Auth");
            }

            ViewBag.HRName = HttpContext.Session.GetString("UserName");
            return View("HRDash");
        }

        // =============================
        // 1. View All Lecturers
        // =============================
        public IActionResult LecturersManager()
        {
            if (!IsHR())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    return RedirectToAction("Login", "Auth");

                return RedirectToAction("AccessDenied", "Auth");
            }

            var lecturers = _context.Lecturers.ToList();
            return View(lecturers);
        }

        // =============================
        // 2. Edit Hourly Rate (GET)
        // =============================
        public IActionResult Rate(int id)
        {
            if (!IsHR())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    return RedirectToAction("Login", "Auth");

                return RedirectToAction("AccessDenied", "Auth");
            }

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();

            return View(lecturer);
        }

        // =============================
        // 3. Edit Hourly Rate (POST)
        // =============================
        [HttpPost]
        public IActionResult Rate(int id, decimal ratePerHour)
        {
            if (!IsHR())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    return RedirectToAction("Login", "Auth");

                return RedirectToAction("AccessDenied", "Auth");
            }

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null)
                return NotFound();

            lecturer.RatePerHour = ratePerHour;
            _context.SaveChanges();

            TempData["Success"] = "Hourly rate updated successfully.";
            return RedirectToAction("LecturersManager");
        }

        // =============================
        // 4. View Approved Claims
        // =============================
        public IActionResult ApprovedClaims()
        {
            if (!IsHR())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    return RedirectToAction("Login", "Auth");

                return RedirectToAction("AccessDenied", "Auth");
            }

            var claims = _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Approved)
                .OrderByDescending(c => c.DateApproved)
                .ToList();

            return View(claims);
        }
    }
}
