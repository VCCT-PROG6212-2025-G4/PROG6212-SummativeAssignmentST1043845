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
        private readonly IWebHostEnvironment _env;

        public ClaimsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// GET: Claims
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(claims);
        }

//--------------------------------------------------------------------------------------------------------------------------------------//


        /// <summary>
        /// GET: Claims/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            return View(claim);
       }

        //---------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// GET: Claims/Create
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClaimVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var lecturerId = HttpContext.Session.GetInt32("LecturerId");
            if (lecturerId == null)
                return RedirectToAction("Login", "Auth");

            /// FILE VALIDATION
            if (vm.Document != null && vm.Document.Length > 0)
            {
                var extension = Path.GetExtension(vm.Document.FileName).ToLower();
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Document", "Only PDF, DOCX, and XLSX files are allowed.");
                    return View(vm); 
                }
            }

            /// SAVE CLAIM
            var claim = new Claim
            {
                LecturerId = lecturerId.Value,
                HoursWorked = (int)vm.HoursWorked,
                Notes = vm.Notes,
                DateSubmitted = DateTime.Now,
                Status = ClaimStatus.Pending
            };

            /// SAVE DOCUMENT
            if (vm.Document != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(vm.Document.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.Document.CopyToAsync(stream);

                claim.SupportingDocumentPath = "/uploads/" + fileName;
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyClaims");
        }



        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// GET: Claims/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            return View(claim);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// POST: Claims/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
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

 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// GET: Claims/Approve/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// GET: Claims/Reject/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

                

 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// GET: Claims/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null) return NotFound();

            return View(claim);
        }

                

 //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// POST: Claims/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        
//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//
