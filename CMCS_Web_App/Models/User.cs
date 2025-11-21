using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS_Web_App.Models
{
    public class User
    {

        public int UserId { get; set; }
        public string? Role { get; set; } // Lecturer / Manager / Coordinator / HR
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Lecturer-specific
        [NotMapped]
        public int? LecturerId { get; set; }

        [NotMapped]
        public string? Department { get; set; }

        [NotMapped]
        [Range(0, 10000)] public decimal? RatePerHour { get; set; }
    }
}

