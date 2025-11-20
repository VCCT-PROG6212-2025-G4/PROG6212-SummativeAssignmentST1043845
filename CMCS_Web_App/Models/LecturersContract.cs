using System.ComponentModel.DataAnnotations;

namespace CMCS_Web_App.Models
{
    public class LecturersContract
    {
        public class LecturerContract
        {
            public int Id { get; set; }

            // Link to Lecturer (if you have a Lecturer table)
            public int LecturerId { get; set; }

            [Required]
            [Range(0, 1000)]
            public decimal HourlyRate { get; set; }

            [Required]
            [Range(0, 200)]
            public int MaxMonthlyHours { get; set; }

            public bool IsActive { get; set; } = true;

            public Lecturer? Lecturer { get; set; }
        }
    }
}

