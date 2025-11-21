using System.ComponentModel.DataAnnotations;
using System.Security.Claims;



namespace CMCS_Web_App.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Range(0, 1000)]
        public decimal RatePerHour { get; set; }

        // Navigation collection must be initialized to avoid nulls
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        public string? PasswordHash { get; set; }
    }
}
