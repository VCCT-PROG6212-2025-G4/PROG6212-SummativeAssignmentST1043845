using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}


