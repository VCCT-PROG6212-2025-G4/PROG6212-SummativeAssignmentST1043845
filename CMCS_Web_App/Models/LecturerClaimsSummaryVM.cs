namespace CMCS_Web_App.Models
{
    public class LecturerClaimsSummaryVM
    {
        public string? LecturerName { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalAmountPaid { get; set; }
    }
}
