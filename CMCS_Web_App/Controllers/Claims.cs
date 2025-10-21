using CMCS_Web_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMCS_Web_App.Models;


namespace CMCS_Web_App.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            return View(claim);
        }

        //// GET: Claims/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Claims/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Claim claim)
        //{
        //    if (!ModelState.IsValid)
        //        return View(claim);

        //    // Handle file upload
        //    if (claim.Document != null && claim.Document.Length > 0)
        //    {
        //        var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
        //        var extension = Path.GetExtension(claim.Document.FileName).ToLower();

        //        if (!allowedExtensions.Contains(extension))
        //        {
        //            ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed.");
        //            return View(claim);
        //        }

        //        var uploadsFolder = Path.Combine("wwwroot/uploads");
        //        Directory.CreateDirectory(uploadsFolder);

        //        var fileName = $"{Guid.NewGuid()}{extension}";
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        using var stream = new FileStream(filePath, FileMode.Create);
        //        await claim.Document.CopyToAsync(stream);

        //        claim.SupportingDocumentPath = "/uploads/" + fileName;
        //    }

        //    claim.DateSubmitted = DateTime.Now;
        //    claim.Status = ClaimStatus.Pending;

        //    _context.Add(claim);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            return View(claim);
        }

        // POST: Claims/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Claim claim)
        {
            if (id != claim.ClaimId) return NotFound();

            if (!ModelState.IsValid)
                return View(claim);

            try
            {
                _context.Update(claim);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Claims.Any(e => e.ClaimId == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Approve/5
        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Reject/5
        [HttpGet]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
