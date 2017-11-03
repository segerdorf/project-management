using System;
using System.Collections.Generic;


namespace RosalieSegerdorf_MVC1.Data.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public User ProjectLeader { get; set; }

        public string Platform { get; set; } 

        public string Description { get; set; }

        public ICollection<User> Employees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsActive { get; set; }
        
    }
}