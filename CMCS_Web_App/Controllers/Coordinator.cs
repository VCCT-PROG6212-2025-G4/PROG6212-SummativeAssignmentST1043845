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
    
    public class CoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Logic managing login for coordinators
        ///  Co-ordinator Dash login method
        ///  Updateded to include session-based role check
        /// </summary> 

        [HttpGet]
        public async Task<IActionResult> CoordDash()
        {
            // Check if user is logged in
            var userId = HttpContext.Session.GetInt32("UserId"); // ✅ use GetInt32
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || string.IsNullOrEmpty(role))
            {
                // Session expired or user not logged in
                return RedirectToAction("Login", "Auth");
            }

            if (role != "Coordinator") // ✅ ensure exact match
            {
                // Logged in but not authorized for this page
                return RedirectToAction("AccessDenied", "Auth");
            }

            // Load all claims for coordinator view
            var claims = await _context.Claims
             .Include(c => c.Lecturer)
             .OrderByDescending(c => c.DateSubmitted)
             .ToListAsync();

            return View (claims); // ✅ ensure /Views/Coordinator/CoordDash.cshtml exists
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Approve claim logic
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
            return RedirectToAction(nameof(CoordDash));
        }

 //------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Reject claim logic
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
            return RedirectToAction(nameof(CoordDash));
        }

 //------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Set pending status 
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
            return RedirectToAction(nameof(CoordDash));
        }
    }
}
//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//
