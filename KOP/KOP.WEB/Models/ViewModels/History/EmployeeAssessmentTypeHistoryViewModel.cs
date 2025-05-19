using KOP.Common.DTOs.AssessmentDTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeAssessmentTypeHistoryViewModel
    {
        public int EmployeeId { get; set; }
        public int AssessmentTypePlanValue { get; set; }
        public List<AssessmentDTO> Assessments { get; set; } = new();
    }
}