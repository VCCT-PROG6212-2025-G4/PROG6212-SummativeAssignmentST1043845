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

    //-----------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// GET: Lecturers
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns></returns>
    [HttpGet("Lecturer/LecturerDash")]
    public IActionResult LecturerDash()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var role = HttpContext.Session.GetString("Role");

        // Check if user is logged in
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
        {
            return RedirectToAction("Login", "Auth");
        }

        // Check if user is authorized as Lecturer
        if (role != "Lecturer")
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        Console.WriteLine("LecturerDash method triggered");

        // Get name from session
        ViewBag.LecturerName = HttpContext.Session.GetString("UserName");

        return View();
    }


    //-----------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// POST:
    /// </summary>
    /// <param name="claim"></param>
    /// <param name="SupportingDocument"></param>
    /// <returns></returns>

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile? SupportingDocument)
    {
        if (!ModelState.IsValid)
            return View(claim);

        
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

        
        claim.DateSubmitted = DateTime.Now;
        claim.Status = ClaimStatus.Pending;

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

       
        return RedirectToAction(nameof(MyClaims), new { firstName = claim.FirstName, lastName = claim.LastName });
    }

 //---------------------------------------------------------------------------------------------------------------//
  
    /// <summary>
    /// GET:
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult SubmitClaim()
    {
   
        return View();
    }

 //------------------------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// My Claims View
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns></returns>


    public async Task<IActionResult> MyClaims(string firstName, string lastName)
    {
        var claims = await _context.Claims
            .Where(c => c.FirstName == firstName && c.LastName == lastName)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();

        if (!claims.Any())
        {
            ViewBag.Message = "No claims found for this lecturer.";
            return View("MyClaims"); 
        }

        ViewBag.LecturerName = $"{firstName} {lastName}";
        return View(claims);
    }


}

//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//


