using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CMCS_Web_App.Controllers
{
    [Authorize]
    public class CoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        // LOGIN (temporary simple login system for POE)
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Simple demo credentials
            if (email == "Co-ordinator@cmcs.com" && password == "67890")
            {
                // Create user claims
              var claims = new List<System.Security.Claims.Claim>
                {
                  new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, email),
                  new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Coordinator")
               };


                var identity = new ClaimsIdentity(claims, "CMCSAuth");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync("CMCSAuth", principal);

                return RedirectToAction("CoordDash", "Coordinator");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        // LOGOUT
        public IActionResult Logout()
        {
            TempData["IsCoordinatorLoggedIn"] = false;
            return RedirectToAction("Login");
        }

        // DASHBOARD
        public async Task<IActionResult> CoordDash()
        {
            if (TempData["IsCoordinatorLoggedIn"] == null || !(bool)TempData["IsCoordinatorLoggedIn"]!)
                return RedirectToAction("Login");

            TempData.Keep("IsCoordinatorLoggedIn");

            var claims = await _context.Claims
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();

            return View("CoordDash", claims);
        }

        // APPROVE CLAIM
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CoordDash));
        }

        // REJECT CLAIM
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CoordDash));
        }

        // SET PENDING (ON HOLD)
        [HttpPost]
        public async Task<IActionResult> SetPending(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Pending;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CoordDash));
        }
    }
}
