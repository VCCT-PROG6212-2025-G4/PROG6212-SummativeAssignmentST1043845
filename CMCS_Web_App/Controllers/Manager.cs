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
        /// Manager dash login
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ManagerDash()
        {
            // SESSION-BASED ROLE CHECK (MANDATORY)
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            // Load claims
            var claims = await _context.Claims
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            return View("ManagerDash", claims);
        }


        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Approve claim logic for manager , to approve a claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagerDash));
        }

 //---------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Reject claim logic for manager , to reject a claim
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManagerDash));
        }

 //---------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Pending claim status logic for manager , to set a claim back to pending
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//

