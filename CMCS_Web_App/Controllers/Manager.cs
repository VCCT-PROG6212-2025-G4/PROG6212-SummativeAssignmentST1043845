using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CMCS_Web_App.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;

        public ManagerController(AppDbContext context)
        {
            _context = context;
        }


 //--------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Manager DASHBOARD
        /// </summary>
        /// <returns></returns>
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

