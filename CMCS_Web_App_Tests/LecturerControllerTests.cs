using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCS_Web_App.Controllers;
using CMCS_Web_App.Data;
using CMCS_Web_App.Models;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Hosting;


namespace CMCS_Web_App_Tests
{
    public class LecturerControllerTests
    {
        private AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("CMCS_LecturerTests")
                .Options;
            return new AppDbContext(options);
        }

        private LecturerController GetController(AppDbContext context)
        {
            var mockEnv = new Mock<IWebHostEnvironment>();
            return new LecturerController(context, mockEnv.Object);
        }

        [Fact]
        
        public async Task MyClaims_ReturnsViewResult()
        {
            var controller = GetController(GetContext());
            var result = await controller.MyClaims();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_AddsLecturer_AndRedirects()
        {
            var context = GetContext();
            var controller = GetController(context);

            // ✅ Initialize all required properties
            var lecturer = new Lecturer
            {
                FirstName = "Lofentse",
                LastName = "Moagi",
               
                Claims = new List<Claim>() // empty list is fine
            };

            var result = await controller.SubmitClaim(lecturer);

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(context.Lecturers);
        }
    }
}