using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.Account
{
    public class LoginViewModel
    {
        public const string RequiredField = "Field is required";

        [Required(ErrorMessage = RequiredField)]
        [Display(Name = "Username/Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}