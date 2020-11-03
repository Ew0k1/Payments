using System.ComponentModel.DataAnnotations;

namespace Payments.WEB.Models.Account
{
    public class PasswordChangeViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}