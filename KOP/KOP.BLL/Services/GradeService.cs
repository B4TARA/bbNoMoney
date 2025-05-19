using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingService _mappingService;

        public GradeService(IUnitOfWork unitOfWork, IMappingService mappingService)
        {
            _unitOfWork = unitOfWork;
            _mappingService = mappingService;
        }

        // Получить количественную оценку по id
        public async Task<IBaseResponse<GradeDTO>> GetGrade(int id)
        {
            try
            {
                // Получаем оценку карьерного роста по id
                var grade = await _unitOfWork.Grades.GetAsync(x => x.Id == id, includeProperties: new string[]
                {
                    "GradeType",
                    "GradeStatus",
                    "Comments.Employee",
                    "EmployeeStateBeforeGrade",
                    "EmployeeStateAfterGrade",
                });

                if (grade == null)
                {
                    return new BaseResponse<GradeDTO>()
                    {
                        Description = $"[GradeService.GetGrade] : Количественная оценка с id = {id} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var gradeDTO = await _mappingService.CreateGradeDTO(grade);

                if (gradeDTO.StatusCode != StatusCodes.OK || gradeDTO.Data == null)
                {
                    return new BaseResponse<GradeDTO>()
                    {
                        Description = gradeDTO.Description,
                        StatusCode = gradeDTO.StatusCode,
                    };
                }

                return new BaseResponse<GradeDTO>()
                {
                    Data = gradeDTO.Data,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GradeDTO>()
                {
                    Description = $"[EmployeeService.GetGrade] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить тип количественных оценок по id
        public async Task<IBaseResponse<GradeTypeDTO>> GetGradeType(int id)
        {
            try
            {
                // Получаем тип количественной оценки по id
                var gradeType = await _unitOfWork.GradeTypes.GetAsync(x => x.Id == id);

                if (gradeType == null)
                {
                    return new BaseResponse<GradeTypeDTO>()
                    {
                        Description = $"[GetGradeType.GetGrade] : Тип количественных оценок с id = {id} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var gradeTypeDTO = _mappingService.CreateGradeTypeDTO(gradeType);

                if (gradeTypeDTO.StatusCode != StatusCodes.OK || gradeTypeDTO.Data == null)
                {
                    return new BaseResponse<GradeTypeDTO>()
                    {
                        Description = gradeTypeDTO.Description,
                        StatusCode = gradeTypeDTO.StatusCode,
                    };
                }

                return new BaseResponse<GradeTypeDTO>()
                {
                    Data = gradeTypeDTO.Data,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GradeTypeDTO>()
                {
                    Description = $"[GradeService.GetGradeType] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить все количественные оценки по типу оценки и сотруднику
        public async Task<IBaseResponse<List<GradeDTO>>> GetGrades(int employeeId, int gradeTypeId)
        {
            try
            {
                // Получаем все количественные оценки по типу и сотруднику
                var grades = await _unitOfWork.Grades.GetAllAsync(x => x.GradeTypeId == gradeTypeId && x.EmployeeId == employeeId, includeProperties: new string[]
                {
                    "GradeType",
                    "GradeStatus",
                    "EmployeeStateBeforeGrade.EmployeeStateAttributes.Attribute",
                    "EmployeeStateAfterGrade.EmployeeStateAttributes.Attribute",
                });

                var gradeDTOs = new List<GradeDTO>();

                foreach (var grade in grades.OrderBy(x => x.DateOfCreation))
                {
                    var gradeDTO = await _mappingService.CreateGradeDTO(grade);

                    if (gradeDTO.StatusCode != StatusCodes.OK || gradeDTO.Data == null)
                    {
                        return new BaseResponse<List<GradeDTO>>()
                        {
                            Description = gradeDTO.Description,
                            StatusCode = gradeDTO.StatusCode,
                        };
                    }

                    gradeDTOs.Add(gradeDTO.Data);
                }

                return new BaseResponse<List<GradeDTO>>()
                {
                    Data = gradeDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<GradeDTO>>()
                {
                    Description = $"[GradeService.GetGrades] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить последнюю количественную оценку по типу оценки и сотруднику
        public async Task<IBaseResponse<int>> GetLastGradeId(int employeeId, int gradeTypeId)
        {
            try
            {
                var grades = await _unitOfWork.Grades.GetAllAsync(x => x.EmployeeId == employeeId && x.GradeTypeId == gradeTypeId);

                var lastGrade = grades.OrderByDescending(x => x.DateOfCreation).FirstOrDefault();

                if (lastGrade == null)
                {
                    return new BaseResponse<int>()
                    {
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                return new BaseResponse<int>()
                {
                    Data = lastGrade.Id,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = $"[GradeService.GetLastGradeId] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить все типы количественных оценок по сотруднику
        public async Task<IBaseResponse<List<GradeTypeDTO>>> GetGradeTypes(int employeeId)
        {
            try
            {
                // Получаем все количественные оценки сотрудника
                var grades = await _unitOfWork.Grades.GetAllAsync(x => x.EmployeeId == employeeId, includeProperties: new string[]
                {
                    "GradeType",
                });

                // Группируем все оценки по "Типу оценок"
                var gradeTypes = grades.GroupBy(x => x.GradeType);

                var gradeTypeDTOs = new List<GradeTypeDTO>();

                foreach (var gradeType in gradeTypes)
                {
                    var gradeTypeDTO = new GradeTypeDTO
                    {
                        Id = gradeType.Key.Id,
                        Name = gradeType.Key.Name,
                        EmployeeId = employeeId,
                    };

                    gradeTypeDTOs.Add(gradeTypeDTO);
                }

                return new BaseResponse<List<GradeTypeDTO>>()
                {
                    Data = gradeTypeDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<GradeTypeDTO>>()
                {
                    Description = $"[GradeService.GetGradeTypes] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}