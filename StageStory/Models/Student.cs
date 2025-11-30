using StageStory.Models.Enum;

namespace StageStory.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public ProfileEnum Profile { get; set; }
        public string? Email { get; set; }
        public string? Level { get; set; }
        public string? Specialization { get; set; }
        public int Year { get; set; }
        public string? University { get; set; }

        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Internship>? Internships { get; set; }
    }
}
