namespace KOP.WEB.Models.ViewModels.Analytics
{
    public class AssessmentAnalyticsViewModel
    {
        public List<string> CompetencesNames { get; set; } = new();
        public List<double> CompetentiesValues { get; set; } = new();
    }
}