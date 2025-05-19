using KOP.Common.DTOs;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<IBaseResponse<EmployeeDTO>> GetEmployee(int id);
        Task<IBaseResponse<string>> GetEmployeeName(int employeeId);
        Task<IBaseResponse<bool>> HasChildRoles(int roleId);
        Task<IBaseResponse<List<AssessmentDTO>>> GetEmployeeLastAssessments(int employeeId, int supervisorId);
        Task<IBaseResponse<List<AssessmentResultDTO>>> GetColleagueAssessmentResults(int employeeId);
        Task<IBaseResponse<AssessmentResultDTO>> GetSelfAssessment(int employeeId, int assessmentId);
        Task<IBaseResponse<object>> AssessEmployee(AssessEmployeeDTO assessEmployeeDTO);
        Task<IBaseResponse<List<GradeDTO>>> GetGradeType(int employeeId, int gradeTypeId);
        Task<BaseResponse<EmployeeStateDTO>> GetEmployeeStateAfterGrade(int gradeId);
    }
}