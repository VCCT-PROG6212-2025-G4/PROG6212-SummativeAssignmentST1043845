using System.ComponentModel.DataAnnotations;

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
    }
}

