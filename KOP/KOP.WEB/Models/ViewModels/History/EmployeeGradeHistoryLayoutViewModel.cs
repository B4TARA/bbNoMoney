using KOP.Common.DTOs.GradeDTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeGradeHistoryLayoutViewModel
    {
        public int EmployeeId { get; set; }
        public List<GradeTypeDTO> GradeTypes { get; set; } = new();
    }
}