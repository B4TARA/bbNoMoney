using KOP.Common.DTOs;
using KOP.Common.Interfaces;

namespace KOP.BLL.Interfaces
{
    public interface IMarkService
    {
        Task<IBaseResponse<List<MarkDTO>>> GetMarks(int employeeId, int markTypeId);
        Task<IBaseResponse<List<MarkTypeDTO>>> GetMarkTypes(int employeeId);
        Task<IBaseResponse<object>> SetMarkValue(int id, int value);
    }
}