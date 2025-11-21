using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class ClaimVM
    {
        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal HoursWorked { get; set; }

        public IFormFile? Document { get; set; }

    }
}
