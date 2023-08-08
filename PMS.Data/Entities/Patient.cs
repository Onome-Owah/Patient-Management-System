using System.ComponentModel.DataAnnotations;
namespace PMS.Data.Entities
{
    public class Patient 
    {
        
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        
        public int Age => (int)(DateTime.Now - Dob).TotalDays / 365;

        [Required]
        public string PhoneNo { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string HouseNo { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Ethnicity { get; set; }

        [Required]
        public string NIN { get; set; }

        [Required]
        public string Email { get; set; }

        // [Required]
        // public string Password { get; set; }

        // 1-N Relationship - a patient has 0 or more issues
    public IList<Issue> Issues {get; set; } = new List<Issue>();

    }

}

