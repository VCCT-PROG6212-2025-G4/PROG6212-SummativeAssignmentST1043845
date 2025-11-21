using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
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

            var users = _context.Users.ToList();
            var lecturers = _context.Lecturers.ToList();

            var model = users.Select(u =>
            {
                var lec = lecturers.FirstOrDefault(x => x.Email == u.Email);

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