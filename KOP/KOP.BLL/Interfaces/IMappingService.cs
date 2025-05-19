using KOP.Common.DTOs;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Interfaces;
using KOP.DAL.Entities;
using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Entities.GradeEntities;

namespace KOP.BLL.Interfaces
{
    public interface IMappingService
    {
        Task<IBaseResponse<EmployeeDTO>> CreateEmployeeDTO(Employee employee);
        Task<IBaseResponse<GradeDTO>> CreateGradeDTO(Grade grade);
        IBaseResponse<MarkDTO> CreateMarkDTO(Mark mark);
        Task<IBaseResponse<AssessmentResultDTO>> CreateAssessmentResultDTO(AssessmentResult result, AssessmentMatrix matrix, int planValue);
        IBaseResponse<GradeTypeDTO> CreateGradeTypeDTO(GradeType gradeType);
    }
}