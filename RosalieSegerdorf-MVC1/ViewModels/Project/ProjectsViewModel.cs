using System;
using System.ComponentModel.DataAnnotations;

namespace RosalieSegerdorf_MVC1.ViewModels.Project
{
    public class ProjectsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ProjectLeader { get; set; }

        [Required]
        public string Platform { get; set; }

        [Required]
        public int AmountOfEmployees { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}