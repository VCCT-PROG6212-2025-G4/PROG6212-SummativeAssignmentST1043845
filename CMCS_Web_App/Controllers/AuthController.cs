using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

            Console.WriteLine("User logged in with role: " + user.Role);

            // 4. Redirect based on role
            return user.Role switch
            {
                "Lecturer" => RedirectToAction("LecturerDash", "Lecturer",
                               new { firstName = user.FirstName, lastName = user.LastName }),

                "Coordinator" => RedirectToAction("CoordinatorDash", "Coordinator"),

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
            return RedirectToAction("Login");
        }
    }


    // ===============================
    // LOGIN VIEW MODEL
    // ===============================
    public class LoginVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
    

