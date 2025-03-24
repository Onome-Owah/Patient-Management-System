using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PMS.Web.Models
{
    public class PatientConcernViewModel
    {
        public SelectList Patients { set; get; }

        [Required(ErrorMessage = "Please select a patient")]
        [Display(Name = "Select Patient")]
        public int PatientId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Issue { get; set; } // Renamed from "Issue" to avoid naming conflict

        // Additional properties from the Issue model
        public string CurrentConcern { get; set; }
        public string Length { get; set; }
        public int Severity { get; set; }
        public string Sensitivities { get; set; }
        public string WhichMedication { get; set; }
        public string Allergies { get; set; }
        public string WhichAllergies { get; set; }
        public string Contraceptions { get; set; }
        public string Resolution { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ResolvedOn { get; set; } = DateTime.MinValue;
        public bool Active { get; set; } = true;
       

        // Additional properties from the Patient model
        public string PatientName { get; set; } // For example
        // Add more properties as needed

         public IFormFile Image { get; set; }

       
    }
}
