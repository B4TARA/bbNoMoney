namespace KOP.WEB.Models.ViewModels.Analytics
{
    public class MarkAnalyticsViewModel
    {
        public List<string> Periods { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Mark> Marks { get; set; } = new();
    }

    public class Mark
    {
        public int SumValue { get; set; }
        public string Period { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Values { get; set; } = new();
    }
}