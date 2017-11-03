using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RosalieSegerdorf_MVC1.Data.Models;

namespace RosalieSegerdorf_MVC1.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public const string RegexInvalidString = "Contains illegal characters";
        public const string RequiredField = "Field is required";

        public string Id { get; set; }
        
        [Required(ErrorMessage = RequiredField)]
        [RegularExpression("^[A-Za-z '`´-åäöÅÄÖ-]+$", ErrorMessage = RegexInvalidString)]
        [StringLength(256, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [RegularExpression("^[A-Za-z '`´-åäöÅÄÖ-]+$", ErrorMessage = RegexInvalidString)]
        [StringLength(256, MinimumLength = 3)]
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
        [StringLength(256, MinimumLength = 3)]
        [RegularExpression("^[0-9A-Za-z '`´,-åäöÅÄÖ]+$", ErrorMessage = RegexInvalidString)]
        public string Address { get; set; }
        
        public List<Competence> Competences { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Employeed since")]
        public DateTime StartOfEmployment { get; set; }

        [DisplayName("Project Leader")]
        public bool IsProjectLeader { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }
        
        [DataType(DataType.MultilineText)]
        public List<Data.Models.Project> Projects { get; set; }

    }
}