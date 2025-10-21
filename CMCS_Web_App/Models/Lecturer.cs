using System.ComponentModel.DataAnnotations;
using System.Security.Claims;



namespace CMCS_Web_App.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        public  string? FirstName { get; set; }

        [Required]
        public  string? LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public required string Department { get; set; }

        [Range(0, 1000)]
        public decimal RatePerHour { get; set; }

        public required ICollection<Claim> Claims { get; set; }
    }
}

