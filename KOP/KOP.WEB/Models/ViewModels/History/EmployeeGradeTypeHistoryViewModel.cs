using KOP.Common.DTOs.GradeDTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeGradeTypeHistoryViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeFullName { get; set; }
        public string EmployeeImagePath { get; set; }
        public List<GradeDTO> Grades { get; set; } = new();
    }
}   