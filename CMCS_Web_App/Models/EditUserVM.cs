using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class EditUserVM
    {
        public int UserId { get; set; }

        // Basic user fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        // Lecturer-specific fields
        public int? LecturerId { get; set; }
        public string? Department { get; set; }
        public decimal? RatePerHour { get; set; }
    }
}

