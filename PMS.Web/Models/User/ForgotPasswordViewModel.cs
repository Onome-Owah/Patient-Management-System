using System.ComponentModel.DataAnnotations;

namespace PMS.Web.Models.User;
public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; }
    
}
