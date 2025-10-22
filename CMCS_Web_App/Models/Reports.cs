namespace CMCS_Web_App.Models
{
    public class ReportsModel
    {
        public int AcceptedClaims { get; set; }
        public int DeniedClaims { get; set; }
        public int PendingClaims { get; set; }
        public int TotalClaims => AcceptedClaims + DeniedClaims + PendingClaims;

    }
}
