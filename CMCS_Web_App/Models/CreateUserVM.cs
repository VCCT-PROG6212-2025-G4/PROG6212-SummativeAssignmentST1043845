using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class CreateUserVM
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string?   Role { get; set; }

        [Required]
        public string? Password { get; set; }

        // Lecturer-specific
        public int? LecturerId { get; set; }           // null if not a lecturer
        public string? Department { get; set; }        // null if not a lecturer
        public decimal? RatePerHour { get; set; }      // null if not a lecturer
    }
}

