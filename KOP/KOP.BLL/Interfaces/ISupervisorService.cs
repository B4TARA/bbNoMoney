using KOP.Common.DTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface ISupervisorService
    {
        Task<IBaseResponse<List<EmployeeDTO>>> GetAppointedGrades(int supervisorId);
        Task<IBaseResponse<List<EmployeeDTO>>> GetSubordinateEmployees(int supervisorId);
        Task<IBaseResponse<ModuleDTO>> GetAllSubordinateModules(int supervisorId);
        Task<IBaseResponse<bool>> GetEmployeeGradeNeed(int supervisorId, int employeeId);
        Task<IBaseResponse<object>> AcceptEmployeeGrade(GradeEmployeeDTO gradeEmployeeDTO);
        Task<IBaseResponse<object>> DeclineEmployeeGrade(GradeEmployeeDTO gradeEmployeeDTO);
        Task<IBaseResponse<List<GradeDTO>>> GetGradeType(int employeeId, int gradeTypeId);
    }
}