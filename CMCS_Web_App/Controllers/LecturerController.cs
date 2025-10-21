using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Claim = CMCS_Web_App.Models.Claim;

public class LecturerController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public LecturerController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // GET: Lecturers
    [HttpGet("Lecturer/LecturerDash")]
    public IActionResult LecturerDash(string firstName, string lastName)
    {
        Console.WriteLine("✅ LecturerDash method triggered");

        ViewBag.LecturerName = $"{firstName} {lastName}";

        return View(); // View can use ViewBag or fetch claims by name
    }



    // ===============================
    // SUBMIT CLAIM (POST)
    // ===============================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile? SupportingDocument)
    {
        if (!ModelState.IsValid)
            return View(claim);

        // Save uploaded file
        if (SupportingDocument != null && SupportingDocument.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{SupportingDocument.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await SupportingDocument.CopyToAsync(stream);
            }

            claim.SupportingDocumentPath = $"/uploads/{fileName}";
        }

        // Set claim details
        claim.DateSubmitted = DateTime.Now;
        claim.Status = ClaimStatus.Pending;

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

        // 🔁 Redirect to MyClaims using first and last name
        return RedirectToAction(nameof(MyClaims), new { firstName = claim.FirstName, lastName = claim.LastName });
    }

    [HttpGet]
    public IActionResult SubmitClaim()
    {
        // If you're no longer using stored lecturers, you don't need to query them
        return View();
    }



    // ===============================
    // MY CLAIMS (renamed from ViewClaims)
    // ===============================
    public async Task<IActionResult> MyClaims(string firstName, string lastName)
    {
        var claims = await _context.Claims
            .Where(c => c.FirstName == firstName && c.LastName == lastName)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();

        if (!claims.Any())
        {
            ViewBag.Message = "No claims found for this lecturer.";
            return View("MyClaims"); // still load the page, just empty
        }

        ViewBag.LecturerName = $"{firstName} {lastName}";
        return View(claims);
    }


}

