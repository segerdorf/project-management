using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.User
{
    public class CreateUserViewModel
    {
        public const string RegexInvalidString = "Contains illegal characters";
        public const string RequiredField = "Field is required";
        public const string PasswordCriterias = "Your password most contain both numbers and letters";
        
        [Key]
        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 6)]
        [RegularExpression("^([0-9]+[a-zA-Z]+|[a-zA-Z]+[0-9]+)[0-9a-zA-Z]*$", ErrorMessage = PasswordCriterias)]
        public string Password { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [RegularExpression("^[A-Za-z åäöÅÄÖ-]+$", ErrorMessage = RegexInvalidString)]
        [StringLength(256, MinimumLength = 3)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [RegularExpression("^[A-Za-z åäöÅÄÖ-]+$", ErrorMessage = RegexInvalidString)]
        [StringLength(256, MinimumLength = 3)]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(15, MinimumLength = 7)]
        [RegularExpression("^[0-9- +]+$", ErrorMessage = RegexInvalidString)]
        public string Phone { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(512, MinimumLength = 3)]
        [RegularExpression("^[0-9A-Za-z ,åäöÅÄÖ]+$", ErrorMessage = RegexInvalidString)]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Start of Employment")]
        public DateTime StartOfEmployment { get; set; }

        [DisplayName("Project Leader")]
        public bool IsProjectLeader { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }
        
    }
}