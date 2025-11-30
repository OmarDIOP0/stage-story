using StageStory.Models.Enum;

namespace StageStory.Models
{
    public class Internship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public string? Evaluation { get; set; }
        public int? SalaryAmount { get; set; }
        public StatusEnum Status { get; set; }

        public ConventionTypeEnum SignatureType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? StudentId { get; set; }
        public int EntrepriseId { get; set; }
        public ProfileEnum Profile { get; set; } = ProfileEnum.Anonymous;
        public virtual Student Student { get; set; }
        public virtual Enterprise Enterprise { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}
