//using CMCS_Web_App.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using CMCS_Web_App.Models;

//namespace CMCS_Web_App.Controllers
//{

//    public class DashboardController : Controller
//    {
//        private readonly AppDbContext _context;

//        public DashboardController(AppDbContext context)
//        {
//            _context = context;
//        }


//        public async Task<IActionResult> Index()
//        {
//            var total = _context.Claims.Count();
//            var approved = _context.Claims.Count(c => c.Status == Models.ClaimStatus.Approved);
//            var pending = _context.Claims.Count(c => c.Status == Models.ClaimStatus.Pending);
//            var rejected = _context.Claims.Count(c => c.Status == Models.ClaimStatus.Rejected);

//            ViewBag.TotalClaims = total;
//            ViewBag.Approved = approved;
//            ViewBag.Pending = pending;
//            ViewBag.Rejected = rejected;

//            // ✅ Include latest claims for display
//            var claims = await _context.Claims
//                .Include(c => c.Lecturer)
//                .OrderByDescending(c => c.ClaimId)
//                .ToListAsync();

//            return View(claims);
//        }
//    }
//}
