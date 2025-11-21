using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS_Web_App.Models
{
    public enum ClaimStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Claim ()
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public int LecturerId { get; set; }

        public Lecturer? Lecturer { get; set; } // Navigation property

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Range(0, 500)]
        public int HoursWorked { get; set; }

        // HR-controlled value. DO NOT show in Lecturer form.
        [Column(TypeName = "decimal(10,2)")]
        public decimal HourlyRate { get; set; }

        public string? ApprovedBy { get; set; }
        public DateTime? DateApproved { get; set; }

        // Auto-calculated (not mapped)
        [NotMapped]
        public decimal TotalAmount => HoursWorked * HourlyRate;

        public string? Notes { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        // File upload
        public string? SupportingDocumentPath { get; set; }

        [NotMapped]
        public IFormFile? Document { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        //public DateTime Date { get; set; }
    }
}

