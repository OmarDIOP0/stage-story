namespace StageStory.Models
{
    public class Enterprise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? SectorActivity { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Internship>? Internships { get; set; }
    }
}
