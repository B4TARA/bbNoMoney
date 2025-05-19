using KOP.Common.DTOs.GradeDTOs;

namespace KOP.WEB.Models.ViewModels.Supervisor
{
    public class EmployeeGradeTypeViewModel
    {
        public List<GradeDTO> Grades { get; set; } = new();
    }
}