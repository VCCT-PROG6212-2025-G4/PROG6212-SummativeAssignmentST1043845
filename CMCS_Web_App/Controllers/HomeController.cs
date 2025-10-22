using CMCS_Web_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CMCS_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //--------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Login credential logic management
        /// I made the home controller the anchor for the login details for coordinator and manager to improve wiring
        /// This logic is to make sure that the coordinator and manager pages are accessed by authorized users only
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            await HttpContext.SignOutAsync("CMCSAuth");


            if (email == "Co-ordinator@cmcs.com" && password == "67890")
            {
                var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, email),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Coordinator")
        };

                var identity = new System.Security.Claims.ClaimsIdentity(claims, "CMCSAuth");
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CMCSAuth", principal);

                return RedirectToAction("CoordDash", "Coordinator");
            }

            if (email == "Manager@cmcs.com" && password == "12345")
            {
                var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, email),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Manager")
        };

                var identity = new System.Security.Claims.ClaimsIdentity(claims, "CMCSAuth");
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CMCSAuth", principal);

                return RedirectToAction("ManagerDash", "Manager");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }
       

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CMCSAuth");
            return RedirectToAction("Login");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//--------------------------------------------------o-o-o-000-END OF FILE-000-o-o-o-------------------------------------------------------------------------------------------------------------//
