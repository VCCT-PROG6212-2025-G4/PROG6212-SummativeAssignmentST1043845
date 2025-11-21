using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    
        public class ApprovedClaimsReportVM
        {
            public List<Claim> Claims { get; set; }
            public List<LecturerClaimsSummaryVM> Summary { get; set; }

            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
    }
    }

