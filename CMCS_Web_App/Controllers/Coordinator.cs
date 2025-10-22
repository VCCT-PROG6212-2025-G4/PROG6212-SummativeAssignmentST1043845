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
    [Authorize(Roles = "Coordinator")]
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
      /// </summary> 
     
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
