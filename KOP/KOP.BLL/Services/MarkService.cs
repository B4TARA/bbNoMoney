using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Entities;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class MarkService : IMarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingService _mappingService;

        public MarkService(IUnitOfWork unitOfWork, IMappingService mappingService)
        {
            _unitOfWork = unitOfWork;
            _mappingService = mappingService;
        }

        // Получить все показатели по типу показателя и сотруднику
        public async Task<IBaseResponse<List<MarkDTO>>> GetMarks(int employeeId, int markTypeId)
        {
            try
            {
                var marks = await _unitOfWork.Marks.GetAllAsync(x => x.EmployeeId == employeeId && x.MarkTypeId == markTypeId, includeProperties: new string[]
                {
                    "MarkType",
                });

                var markDTOs = new List<MarkDTO>();

                foreach (var mark in marks)
                {
                    var markDTO = _mappingService.CreateMarkDTO(mark);

                    if (markDTO.StatusCode != StatusCodes.OK || markDTO.Data == null)
                    {
                        return new BaseResponse<List<MarkDTO>>()
                        {
                            Description = markDTO.Description,
                            StatusCode = markDTO.StatusCode,
                        };
                    }

                    markDTOs.Add(markDTO.Data);
                }

                return new BaseResponse<List<MarkDTO>>()
                {
                    Data = markDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<MarkDTO>>()
                {
                    Description = $"[MarkService.GetMarks] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить все типы показателей по сотруднику
        public async Task<IBaseResponse<List<MarkTypeDTO>>> GetMarkTypes(int employeeId)
        {
            try
            {
                // Получаем все показатели сотрудника
                var marks = await _unitOfWork.Marks.GetAllAsync(x => x.EmployeeId == employeeId, includeProperties: new string[]
                {
                    "MarkType",
                });

                // Группируем все показатели по "Типу показателя"
                var markTypes = marks.GroupBy(x => x.MarkType);

                var markTypeDTOs = new List<MarkTypeDTO>();

                foreach (var markType in markTypes)
                {
                    var markTypeDTO = new MarkTypeDTO
                    {
                        Id = markType.Key.Id,
                        Name = markType.Key.Name,
                        EmployeeId = employeeId,
                    };

                    markTypeDTOs.Add(markTypeDTO);
                }

                return new BaseResponse<List<MarkTypeDTO>>()
                {
                    Data = markTypeDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<MarkTypeDTO>>()
                {
                    Description = $"[MarkService.GetMarkTypes] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Изменить значение показателя
        public async Task<IBaseResponse<object>> SetMarkValue(int id, int value)
        {
            try
            {
                // Получаем показатель
                var mark = await _unitOfWork.Marks.GetAsync(x => x.Id == id, includeProperties: new string[]
                {
                    "MarkType",
                });

                if (mark == null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = $"[MarkService.SetMarkValue] : Показатель с id = {id} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                mark.FactValue = value;

                if(value >= mark.MarkType.PlanValue)
                {
                    mark.Result = true;
                }
                else
                {
                    mark.Result = false;
                }

                _unitOfWork.Marks.Update(mark);

                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[MarkService.SetMarkValue] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}