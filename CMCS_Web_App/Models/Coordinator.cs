using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class Coordinator
    {
        public int CoordinatorId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}


