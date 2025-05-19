using KOP.Common.DTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface IReportService
    {
        Task<IBaseResponse<List<EmployeeDTO>>> GetGradesReport(int supervisorId);

        Task<IBaseResponse<MemoryStream>> SaveGradesReport(int supervisorId);
    }
}