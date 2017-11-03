
namespace RosalieSegerdorf_MVC1.Data.Models
{
    public class Competence
    {
        public int Id { get; set; }

        public Expertise Expertise { get; set; }

        public int YearOfExperience { get; set; }

        public User User { get; set; }
    }
}