using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface IAnalyticsService
    {
        Task<IBaseResponse<List<AssessmentDTO>>> GetEmployeeAssessmentAnalytics(int employeeId);
    }
}