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
        
        /// <summary>
        /// Lecturer-specific
        /// </summary>
        public int? LecturerId { get; set; }           
        public string? Department { get; set; }        
        public decimal? RatePerHour { get; set; }      
    }
}

