using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static CMCS_Web_App.Data.AppDbContext;

namespace CMCS_Web_App.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ===============================
        // LOGIN PAGE (GET)
        // ===============================

        [HttpGet("")]

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        // ===============================
        // LOGIN PAGE (POST)
        // ===============================
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // 1. Find user by email (shared across HR, Lecturer, Coordinator, Manager)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == vm.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(vm);
            }

            // 2. Validate password
            var hashed = PasswordHasher.Hash(vm.Password);

            if (!string.Equals(hashed, user.PasswordHash, StringComparison.Ordinal))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(vm);
            }

            // 3. Store session values
            HttpContext.Session.SetInt32("UserId", user.UserId);

            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

            if (user.Role == "Lecturer")
            {
                var lecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Email == user.Email);
                if (lecturer != null)
                {
                    HttpContext.Session.SetInt32("LecturerId", lecturer.LecturerId);
                }
            }
            
            Console.WriteLine("User logged in with role: " + user.Role);

            // 4. Redirect based on role
            return user.Role switch
            {
                "Lecturer" => RedirectToAction("LecturerDash", "Lecturer"),
                              
                "Coordinator" => RedirectToAction("CoordDash", "Coordinator"),

                "Manager" => RedirectToAction("ManagerDash", "Manager"),

                "HR" => RedirectToAction("HRDash", "HR"),

                _ => RedirectToAction("Login")
            };
        }

        // ===============================
        // LOGOUT
        // ===============================
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
       }


        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
       }

      
    }
}
    

