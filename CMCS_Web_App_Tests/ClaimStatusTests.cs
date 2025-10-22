using Xunit;
using CMCS_Web_App.Models;

namespace CMCS_Web_App.Tests
{
    public class ClaimStatusTests
    {
        [Fact]
        public void NewClaim_ShouldBePending()
        {
            var claim = new Claim { HoursWorked = 4, HourlyRate = 100 };
            Assert.Equal(ClaimStatus.Pending, claim.Status);
        }

        [Fact]
        public void CanUpdateClaimStatus_ToApproved()
        {
            var claim = new Claim { HoursWorked = 4, HourlyRate = 100 };
            claim.Status = ClaimStatus.Approved;
            Assert.Equal(ClaimStatus.Approved, claim.Status);
        }
    }
}
