using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class ClaimVM
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Description { get; set; }

        public string? Notes { get; set; }
        [Required]
        public decimal HoursWorked { get; set; }

        public IFormFile? Document { get; set; }

    }
}
