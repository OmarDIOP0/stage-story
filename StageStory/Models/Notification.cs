using StageStory.Models.Enum;

namespace StageStory.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int StudentId { get; set; }
        public int? InternshipId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Internship Internship { get; set; }

    }
}
