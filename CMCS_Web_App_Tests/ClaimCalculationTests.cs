using Xunit;
using CMCS_Web_App.Models;

namespace CMCS_Web_App.Tests
{
    public class ClaimCalculationTests
    {
        [Theory]
        [InlineData(5, 200, 1000)]
        [InlineData(10, 100, 1000)]
        [InlineData(8, 150, 1200)]
        public void TotalAmount_CalculatesCorrectly(int hours, decimal rate, decimal expected)
        {
            var claim = new Claim { HoursWorked = hours, HourlyRate = rate };
            Assert.Equal(expected, claim.TotalAmount);
        }
    }
}
