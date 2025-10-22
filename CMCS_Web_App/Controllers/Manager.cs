using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CMCS_Web_App.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;

        public ManagerController(AppDbContext context)
        {
            _context = context;
        }

        // LOGIN (temporary simple login system for POE)
       
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Simple demo credentials
            if (email == "Manager@cmcs.com" && password == "12345")
            {
                // Create user claims
                var claims = new List<System.Security.Claims.Claim>
                {
                  new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, email),
                  new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Manager")
               };


                var identity = new ClaimsIdentity(claims, "CMCSAuth");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync("CMCSAuth", principal);

                return RedirectToAction("ManagerDash", "Manager");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        // LOGOUT
        public IActionResult Logout()
        {
            TempData["IsManagerLoggedIn"] = false;
            return RedirectToAction("Login");
        }

        // DASHBOARD
        public async Task<IActionResult> ManagerDash()
        {
            if (TempData["IsManagerLoggedIn"] == null || !(bool)TempData["IsManagerLoggedIn"]!)
                return RedirectToAction("Login");

            TempData.Keep("IsManagerLoggedIn");

            var claims = await _context.Claims
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();

            return View("ManagerDash", claims);
        }

        // APPROVE CLAIM
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagerDash));
        }

        // REJECT CLAIM
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagerDash));
        }

        // SET PENDING (ON HOLD)
        [HttpPost]
        public async Task<IActionResult> SetPending(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Pending;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagerDash));
        }
    }
}

