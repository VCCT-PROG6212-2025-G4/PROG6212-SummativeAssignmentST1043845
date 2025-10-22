using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CMCS_Web_App.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reports/Index
        public async Task<IActionResult> Index()
        {
            var totalClaims = await _context.Claims.CountAsync();
            var approvedClaims = await _context.Claims.CountAsync(c => c.Status == ClaimStatus.Approved);
            var pendingClaims = await _context.Claims.CountAsync(c => c.Status == ClaimStatus.Pending);
            var rejectedClaims = await _context.Claims.CountAsync(c => c.Status == ClaimStatus.Rejected);

            ViewBag.TotalClaims = totalClaims;
            ViewBag.ApprovedClaims = approvedClaims;
            ViewBag.PendingClaims = pendingClaims;
            ViewBag.RejectedClaims = rejectedClaims;

            // Optionally pass claims to view
            var allClaims = await _context.Claims
                .OrderByDescending(c => c.DateSubmitted)
                .ToListAsync();

            return View(allClaims);
        }
    }
}
    