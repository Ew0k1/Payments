using System.ComponentModel.DataAnnotations;

namespace Payments.WEB.Models.Account
{
    public class ResetPasswordViewModel
    {
        //public string UserId { get; set; }

        //public string Token { get; set; }

        //[Required(ErrorMessage = "Field must be filled")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Field must be filled")]
        //[Display(Name = "Confirm Password")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Passwords don't match")]
        //public string ConfirmPassword { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}