using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.Project
{
    public class CreateProjectViewModel
    {
        public const string RequiredField = "Field is required";

        public int Id { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string ProjectLeaderId { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        public string Platform { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(int.MaxValue, MinimumLength = 20)]
        public string Description { get; set; }

        public IEnumerable<string> EmployeeIds { get; set; } 

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Created at")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Release date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Active preoject")]
        public bool IsActive { get; set; }
    }
}