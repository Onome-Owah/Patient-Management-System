using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMS.Data.Entities
{
    // used in ticket search feature
public enum IssueRange { OPEN, CLOSED, ALL }

    public class Issue
    {
        public int IssueId { get; set; }

        [Required]
        public string CurrentConcern { get; set; }

        [Required]
        public string Length { get; set; }

        [Required]
        public int Severity { get; set; }

        
        public string PhotoUrl { get; set; }   

        [Required]
        public string Sensitivities { get; set; }

        [Required]
        public string WhichMedication { get; set; }

        [Required]
        public string Allergies { get; set; }

        [Required]
        public string WhichAllergies { get; set; }

        [Required]
        public string Contraceptions { get; set; }

        [StringLength(500)]
        public string Resolution { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ResolvedOn { get; set; } = DateTime.MinValue;

        public bool Active { get; set; } = true;

        // issue owned by a patient
        // [ForeignKey("Patient")]
        public int PatientId { get; set; }      // foreign key
        //[JsonIgnore] 
        public Patient Patient { get; set; }    // navigation property
        

    }
}