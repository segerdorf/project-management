using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.Project
{
    public class ProjectDetailsViewModel
    {
        public const string RequiredField = "Field is required";

        public int Id { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        public string Title { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        [DisplayName("Project leader")]
        public string ProjectLeader { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(256, MinimumLength = 3)]
        public string Platform { get; set; }

        [Required(ErrorMessage = RequiredField)]
        [StringLength(int.MaxValue, MinimumLength = 20)]
        public string Description { get; set; }
        
        public ICollection<Data.Models.User> Employees { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Created at")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Release date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}