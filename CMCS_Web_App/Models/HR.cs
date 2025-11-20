using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class HR
    {
        [Key]
        public int HRId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Password stored in user table, but we allow this here if needed.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Optional: HR can manage lecturers
        public ICollection<Lecturer>? ManagedLecturers { get; set; }
    }
}
    