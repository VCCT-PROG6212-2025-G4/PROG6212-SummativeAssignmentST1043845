using Xunit;
using CMCS_Web_App.Models;

namespace CMCS_Web_App_Tests
{
    public class ClaimModelTests
    {
        [Fact]
        public void TotalAmount_ShouldBe_HoursWorkedTimesHourlyRate()
        {
            var claim = new Claim { HoursWorked = 10, HourlyRate = 150 };
            Assert.Equal(1500, claim.TotalAmount);
        }

        [Fact]
        public void DefaultStatus_ShouldBe_Pending()
        {
            var claim = new Claim { HoursWorked = 5, HourlyRate = 200 };
            Assert.Equal(ClaimStatus.Pending, claim.Status);
        }
    }
}