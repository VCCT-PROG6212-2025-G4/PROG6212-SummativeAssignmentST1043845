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
        var lecturerId = HttpContext.Session.GetInt32("LecturerId");
        var role = HttpContext.Session.GetString("Role");

        // Check if user is logged in
        if (lecturerId == null || string.IsNullOrEmpty(role))
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

        // Optional: Load claims or lecturer info here

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

        claim.LecturerId = HttpContext.Session.GetInt32("LecturerId") ?? 0;

        _context.Claims.Add(claim);
               
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error saving claim: " + ex.Message);
            return View(claim);
        }



        return RedirectToAction("MyClaims");
    }

    //---------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// GET:
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult SubmitClaim()
    {
        // 1. Get the logged-in lecturer's ID from Session
        var lecturerId = HttpContext.Session.GetInt32("LecturerId");

        if (lecturerId == null)
            return RedirectToAction("Login", "Auth");

        // 2. Retrieve lecturer from the database
        var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == lecturerId);

        if (lecturer == null)
            return RedirectToAction("Login", "Auth");

        // 3. Pre-fill the ViewModel
        var vm = new ClaimVM
        {
            FirstName = lecturer.FirstName,
            LastName = lecturer.LastName
        };

        // 4. Pass VM to the view
        return View(vm);
    }


    //------------------------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// My Claims View
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns></returns>


    public async Task<IActionResult> MyClaims()
    {
        var lecturerId = HttpContext.Session.GetInt32("LecturerId");
        if (lecturerId == null)
            return RedirectToAction("Login", "Auth");

        var claims = await _context.Claims
            .Where(c => c.LecturerId == lecturerId)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();

        if (!claims.Any())
        {
            ViewBag.Message = "No claims found for this lecturer.";
            return View("MyClaims", claims); // still pass empty list
        }

        var lecturer = await _context.Lecturers.FindAsync(lecturerId);
        ViewBag.LecturerName = lecturer != null
            ? $"{lecturer.FirstName} {lecturer.LastName}"
            : "Unknown Lecturer";

        return View(claims);
    }


}

//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//


