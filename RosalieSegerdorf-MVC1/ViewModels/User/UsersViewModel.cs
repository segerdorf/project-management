using System.Collections.Generic;
using RosalieSegerdorf_MVC1.Data.Models;

namespace RosalieSegerdorf_MVC1.ViewModels.User
{
    public class UsersViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<Competence> Competences { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public List<Data.Models.Project> ProjectLeaderFor { get; set; }
    }
}