using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface IGradeService
    {
        Task<IBaseResponse<GradeDTO>> GetGrade(int id);
        Task<IBaseResponse<GradeTypeDTO>> GetGradeType(int id);
        Task<IBaseResponse<List<GradeDTO>>> GetGrades(int employeeId, int gradeTypeId);
        Task<IBaseResponse<int>> GetLastGradeId(int employeeId, int gradeTypeId);
        Task<IBaseResponse<List<GradeTypeDTO>>> GetGradeTypes(int employeeId);
    }
}