using KOP.Common.DTOs.AssessmentDTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeAssessmentHistoryLayoutViewModel
    {
        public int EmployeeId { get; set; }
        public List<AssessmentTypeDTO> AssessmentTypes { get; set; } = new();
    }
}