using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMS.Web.Models
{
    public class IssueCreateViewModel
    {
        public SelectList Patients { set; get; }

        [Required(ErrorMessage = "Please select a patient")]
        [Display(Name = "Select Patient")]
        public int PatientId { get; set; }

        
        // public string Issue { get; set; } // Renamed from "Issue" to avoid naming conflict

        // Additional properties from the Issue model
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string CurrentConcern { get; set; }

        [Required]
        public string Length { get; set; }

        [Required]
        public int Severity { get; set; }
        public string PhotoUrl { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Sensitivities { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string WhichMedication { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Allergies { get; set; }

       
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string WhichAllergies { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Contraceptions { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Resolution { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ResolvedOn { get; set; } = DateTime.MinValue;
        public bool Active { get; set; } = true;
       

        // Additional properties from the Patient model
        public string PatientName { get; set; } // For example
        // Add more properties as needed

        // Constructor to initialize the SelectList
        public IssueCreateViewModel(SelectList patients)
        {
            Patients = patients;
        }
    }
}
