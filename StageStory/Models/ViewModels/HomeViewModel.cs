using StageStory.Models.Enum;

namespace StageStory.Models.ViewModels
{
    public class HomeViewModel
    {
        public Internship NewInternship { get; set; } = new Internship();
        public List<Internship> RecentInternships { get; set; } = new List<Internship>();
        public List<Internship> AllInternships { get; set; } = new List<Internship>();

        // Statistiques
        public int TotalInternships => AllInternships.Count;
        public int ApprovedInternships => AllInternships.Count(i => i.Status == StatusEnum.Approved);
        public int PendingInternships => AllInternships.Count(i => i.Status == StatusEnum.Pending);
        public int RejectedInternships => AllInternships.Count(i => i.Status == StatusEnum.Rejected);
        public double AverageRating => (double)AllInternships.Where(i => i.Rating > 0).Average(i => i.Rating);
    }
}
