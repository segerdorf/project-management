using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using RosalieSegerdorf_MVC1.Data.Models;

namespace RosalieSegerdorf_MVC1.ViewModels.Competences
{
    public class CreateCompetenceViewModel
    {
        public const string RequiredField = "Field is required";

        public int Id { get; set; }
        
        [Required]
        public Expertise Expertise { get; set; }
        
        [Required(ErrorMessage = RequiredField)]
        [DisplayName("Years of experience")]
        [Range(minimum:0, maximum:100)]
        public int YearOfExperience { get; set; }

        public string UserId { get; set; }

    }
}