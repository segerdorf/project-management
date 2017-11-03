using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.User
{
    public class ResetPassword
    {
        public const string RequiredPassword = "Type your password";
        public const string CompareRequiredPassword = "Confirm your password";
        public const string WrongPassword = "Wrong password";
        public const string PasswordCriterias = "Your password most contain both numbers and letters";

        [Required(ErrorMessage = RequiredPassword)]
        [DisplayName("Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = RequiredPassword)]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 6)]
        [RegularExpression("^([0-9]+[a-zA-Z]+|[a-zA-Z]+[0-9]+)[0-9a-zA-Z]*$", ErrorMessage = PasswordCriterias)]
        public string Password { get; set; }

        [Required(ErrorMessage = CompareRequiredPassword)]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [CompareAttribute("Password", ErrorMessage = WrongPassword)]
        public string ConfirmPassword { get; set; }

    }
}