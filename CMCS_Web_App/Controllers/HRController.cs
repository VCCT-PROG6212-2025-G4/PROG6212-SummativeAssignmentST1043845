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
            // Ensure user is authenticated
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return RedirectToAction("Login", "Auth");

            // Ensure user is HR
            if (!IsHR())
                return RedirectToAction("AccessDenied", "Auth");

            // Get all users with role "Lecturer"
            var lecturerUsers = _context.Users
                .Where(u => u.Role == "Lecturer")
                .ToList();

            return View(lecturerUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(Lecturer lecturer)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            if (ModelState.IsValid)
            {
                // Hash password
                var hashedPassword = PasswordHasher.Hash(lecturer.PasswordHash ?? "");

                // Create User
                var user = new User
                {
                    Email = lecturer.Email,
                    FirstName = lecturer.FirstName,
                    LastName = lecturer.LastName,
                    Role = "Lecturer",
                    PasswordHash = hashedPassword
                };

                // Update Lecturer's PasswordHash
                lecturer.PasswordHash = hashedPassword;

                _context.Users.Add(user);
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();

                return RedirectToAction("LecturersManager");
            }

            return View(lecturer);
        }

        public IActionResult EditLecturer(int id)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer == null) return NotFound();

            return View(lecturer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditLecturer(Lecturer lecturer)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            if (ModelState.IsValid)
            {
                // Update Lecturer
                _context.Lecturers.Update(lecturer);

                // Also update matching User (by email)
                var user = _context.Users.FirstOrDefault(u => u.Email == lecturer.Email);
                if (user != null)
                {
                    user.FirstName = lecturer.FirstName;
                    user.LastName = lecturer.LastName;
                    user.Email = lecturer.Email;
                }

                _context.SaveChanges();
                return RedirectToAction("LecturersManager");
            }

            return View(lecturer);
        }

        [HttpPost]
        public IActionResult DeleteLecturer(int id)
        {
            if (!IsHR()) return RedirectToAction("AccessDenied", "Auth");

            var lecturer = _context.Lecturers.Find(id);
            if (lecturer != null)
            {
                // Find matching User by email
                var user = _context.Users.FirstOrDefault(u => u.Email == lecturer.Email && u.Role == "Lecturer");

                if (user != null)
                    _context.Users.Remove(user);

                _context.Lecturers.Remove(lecturer);
                _context.SaveChanges();
            }

            return RedirectToAction("LecturersManager");
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
