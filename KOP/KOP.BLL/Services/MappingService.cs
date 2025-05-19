using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Entities;
using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Entities.RelationEntities;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class MappingService : IMappingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MappingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Преобразование Employee в EmployeeDTO
        public async Task<IBaseResponse<EmployeeDTO>> CreateEmployeeDTO(Employee employee)
        {
            try
            {
                if (employee.Role == null)
                {
                    return new BaseResponse<EmployeeDTO>()
                    {
                        Description = $"[MappingService.CreateEmployeeDTO] : Employee.Role is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new EmployeeDTO()
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    ImagePath = employee.ImagePath,
                    Role = employee.Role.Name,
                };

                var gradeTypes = employee.Grades.GroupBy(x => x.GradeType);

                foreach (var gradeType in gradeTypes)
                {
                    var lastGrade = gradeType.OrderByDescending(x => x.DateOfCreation).First();

                    var gradeDTO = await CreateGradeDTO(lastGrade);

                    if (gradeDTO.StatusCode != StatusCodes.OK || gradeDTO.Data == null)
                    {
                        return new BaseResponse<EmployeeDTO>()
                        {
                            Description = gradeDTO.Description,
                            StatusCode = gradeDTO.StatusCode,
                        };
                    }

                    dto.LastGrades.Add(gradeDTO.Data);
                }

                return new BaseResponse<EmployeeDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeeDTO>()
                {
                    Description = $"[MappingService.CreateEmployeeDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование Grade в GradeDTO
        public async Task<IBaseResponse<GradeDTO>> CreateGradeDTO(Grade grade)
        {
            try
            {
                if (grade.GradeStatus == null)
                {
                    return new BaseResponse<GradeDTO>()
                    {
                        Description = $"[MappingService.CreateGradeDTO] : Grade.GradeStatus is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new GradeDTO()
                {
                    Id = grade.Id,
                    GradeTypeId = grade.GradeTypeId,
                    EmployeeId = grade.EmployeeId,
                    Number = grade.Number,
                    GradeTypeName = grade.GradeType.Name,
                    GradeStatusName = grade.GradeStatus.Name,
                    HtmlClassName = grade.GradeStatus.HtmlClassName,
                    StartDate = grade.StartDate,
                    EndDate = grade.EndDate,
                    NextGradeDate = grade.NextGradeDate,
                    DateOfCreation = grade.DateOfCreation,
                    
                    IsClickable = true,
                };

                if (grade.EmployeeStateBeforeGrade != null)
                {
                    var stateBeforeGradeDTO = await CreateEmployeeStateDTO(grade.EmployeeStateBeforeGrade);

                    if (stateBeforeGradeDTO.StatusCode != StatusCodes.OK || stateBeforeGradeDTO.Data == null)
                    {
                        return new BaseResponse<GradeDTO>()
                        {
                            Description = stateBeforeGradeDTO.Description,
                            StatusCode = stateBeforeGradeDTO.StatusCode,
                        };
                    }

                    dto.EmployeeStateBeforeGrade = stateBeforeGradeDTO.Data;
                }

                if (grade.EmployeeStateAfterGrade != null)
                {
                    var stateAfterGradeDTO = await CreateEmployeeStateDTO(grade.EmployeeStateAfterGrade);

                    if (stateAfterGradeDTO.StatusCode != StatusCodes.OK || stateAfterGradeDTO.Data == null)
                    {
                        return new BaseResponse<GradeDTO>()
                        {
                            Description = stateAfterGradeDTO.Description,
                            StatusCode = stateAfterGradeDTO.StatusCode,
                        };
                    }

                    dto.EmployeeStateAfterGrade = stateAfterGradeDTO.Data;
                }

                foreach (var comment in grade.Comments)
                {
                    var commentDTO = CreateCommentDTO(comment);

                    if (commentDTO.StatusCode != StatusCodes.OK || commentDTO.Data == null)
                    {
                        return new BaseResponse<GradeDTO>()
                        {
                            Description = commentDTO.Description,
                            StatusCode = commentDTO.StatusCode,
                        };
                    }

                    dto.Comments.Add(commentDTO.Data);
                }

                return new BaseResponse<GradeDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GradeDTO>()
                {
                    Description = $"[MappingService.CreateGradeDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование Grade в GradeDTO
        public IBaseResponse<GradeTypeDTO> CreateGradeTypeDTO(GradeType gradeType)
        {
            try
            {
                var dto = new GradeTypeDTO()
                {
                    Id = gradeType.Id,
                    NumberOfGrades = gradeType.NumberOfGrades,
                };

                return new BaseResponse<GradeTypeDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<GradeTypeDTO>()
                {
                    Description = $"[MappingService.CreateGradeTypeDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование Grade в GradeDTO
        public IBaseResponse<CommentDTO> CreateCommentDTO(Comment comment)
        {
            try
            {
                if (comment.Employee == null)
                {
                    return new BaseResponse<CommentDTO>()
                    {
                        Description = $"[MappingService.CreateCommentDTO] : Comment.Employee is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new CommentDTO()
                {
                    SupervisorId = comment.Employee.Id,
                    SupervisorName = comment.Employee.FullName,
                    Text = comment.Text,
                    IsFeedback = comment.IsFeedback,
                    DateOfCreation = comment.DateOfCreation,
                };

                return new BaseResponse<CommentDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CommentDTO>()
                {
                    Description = $"[MappingService.CreateCommentDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование Mark в MarkDTO
        public IBaseResponse<MarkDTO> CreateMarkDTO(Mark mark)
        {
            try
            {
                if (mark.MarkType == null)
                {
                    return new BaseResponse<MarkDTO>()
                    {
                        Description = $"[MappingService.CreateMarkDTO] : Mark.MarkType is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new MarkDTO()
                {
                    Id = mark.Id,
                    PlanValue = mark.MarkType.PlanValue,
                    FactValue = mark.FactValue,
                    Result = mark.Result,
                    IsPercentage = mark.MarkType.IsPercentage,
                    TypeName = mark.MarkType.Name,
                    Period = mark.Period,
                };

                if(dto.IsPercentage)
                {
                    dto.PercentageValue = Convert.ToInt32(Math.Round((double)dto.FactValue / dto.PlanValue, 2) * 100);
                }

                return new BaseResponse<MarkDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MarkDTO>()
                {
                    Description = $"[MappingService.CreateMarkDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование Assessment в AssessmentDTO
        public async Task<IBaseResponse<AssessmentDTO>> CreateAssessmentDTO(Assessment assessment)
        {
            try
            {
                if (assessment.AssessmentType == null)
                {
                    return new BaseResponse<AssessmentDTO>()
                    {
                        Description = $"[MappingService.CreateAssessmentDTO] : Assessment.AssessmentType is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }
                else if (assessment.AssessmentType.AssessmentMatrix == null)
                {
                    return new BaseResponse<AssessmentDTO>()
                    {
                        Description = $"[MappingService.CreateAssessmentDTO] : Assessment.AssessmentType.AssessmentMatrix is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new AssessmentDTO()
                {
                    Id = assessment.Id,
                    Number = assessment.Number,
                    EmployeeId = assessment.EmployeeId,
                    SystemStatus = assessment.SystemStatus,
                    NextAssessmentDate = assessment.NextAssessmentDate,
                };

                foreach (var result in assessment.AssessmentResults)
                {
                    var resultDTO = await CreateAssessmentResultDTO(result, assessment.AssessmentType.AssessmentMatrix, assessment.AssessmentType.PlanValue);

                    if (resultDTO.StatusCode != StatusCodes.OK || resultDTO.Data == null)
                    {
                        return new BaseResponse<AssessmentDTO>()
                        {
                            Description = resultDTO.Description,
                            StatusCode = resultDTO.StatusCode,
                        };
                    }

                    dto.AssessmentResults.Add(resultDTO.Data);
                }

                return new BaseResponse<AssessmentDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AssessmentDTO>()
                {
                    Description = $"[MappingService.CreateAssessmentDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование AssessmentResult в AssessmentResultDTO
        public async Task<IBaseResponse<AssessmentResultDTO>> CreateAssessmentResultDTO(AssessmentResult result, AssessmentMatrix matrix, int planValue)
        {
            try
            {
                if (result.Judge == null)
                {
                    return new BaseResponse<AssessmentResultDTO>()
                    {
                        Description = $"[MappingService.CreateAssessmentResultDTO] : AssessmentResult.Judge is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }
                else if (result.Judged == null)
                {
                    return new BaseResponse<AssessmentResultDTO>()
                    {
                        Description = $"[MappingService.CreateAssessmentResultDTO] : AssessmentResult.Judged is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new AssessmentResultDTO()
                {
                    Id = result.Id,
                    SystemStatus = result.SystemStatus,
                    Sum = result.AssessmentResultValues.Sum(x => x.Value),
                };

                if(dto.Sum >= planValue)
                {
                    dto.IsPassed = true;
                }

                var judgeDTO = await CreateEmployeeDTO(result.Judge);

                if (judgeDTO.StatusCode != StatusCodes.OK || judgeDTO.Data == null)
                {
                    return new BaseResponse<AssessmentResultDTO>()
                    {
                        Description = judgeDTO.Description,
                        StatusCode = judgeDTO.StatusCode,
                    };
                }

                dto.Judge = judgeDTO.Data;

                var judgedDTO = await CreateEmployeeDTO(result.Judged);

                if (judgedDTO.StatusCode != StatusCodes.OK || judgedDTO.Data == null)
                {
                    return new BaseResponse<AssessmentResultDTO>()
                    {
                        Description = judgedDTO.Description,
                        StatusCode = judgedDTO.StatusCode,
                    };
                }

                dto.Judged = judgedDTO.Data;

                foreach (var value in result.AssessmentResultValues)
                {
                    var valueDTO = CreateAssessmentResultValueDTO(value);

                    if (valueDTO.StatusCode != StatusCodes.OK || valueDTO.Data == null)
                    {
                        return new BaseResponse<AssessmentResultDTO>()
                        {
                            Description = valueDTO.Description,
                            StatusCode = valueDTO.StatusCode,
                        };
                    }

                    dto.Values.Add(valueDTO.Data);
                }

                foreach (var element in matrix.Elements)
                {
                    var elementDTO = CreateAssessmentMatrixElementDTO(element);

                    if (elementDTO.StatusCode != StatusCodes.OK || elementDTO.Data == null)
                    {
                        return new BaseResponse<AssessmentResultDTO>()
                        {
                            Description = elementDTO.Description,
                            StatusCode = elementDTO.StatusCode,
                        };
                    }

                    dto.Elements.Add(elementDTO.Data);
                }

                dto.ElementsByRow = dto.Elements.GroupBy(x => x.Row).ToList();

                dto.MaxValue = matrix.MaxAssessmentMatrixResultValue;
                dto.MinValue = matrix.MinAssessmentMatrixResultValue;

                return new BaseResponse<AssessmentResultDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AssessmentResultDTO>()
                {
                    Description = $"[MappingService.CreateAssessmentResultDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование AssessmentResultValue в AssessmentResultValueDTO
        public IBaseResponse<AssessmentResultValueDTO> CreateAssessmentResultValueDTO(AssessmentResultValue value)
        {
            try
            {
                var dto = new AssessmentResultValueDTO()
                {
                    Value = value.Value,
                    AssessmentMatrixRow = value.AssessmentMatrixRow,
                };

                return new BaseResponse<AssessmentResultValueDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AssessmentResultValueDTO>()
                {
                    Description = $"[MappingService.CreateAssessmentResultValueDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование AssessmentMatrixElement в AssessmentMatrixElementDTO
        public IBaseResponse<AssessmentMatrixElementDTO> CreateAssessmentMatrixElementDTO(AssessmentMatrixElement element)
        {
            try
            {
                var dto = new AssessmentMatrixElementDTO()
                {
                    Row = element.Row,
                    Value = element.Value,
                };

                return new BaseResponse<AssessmentMatrixElementDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AssessmentMatrixElementDTO>()
                {
                    Description = $"[MappingService.CreateAssessmentMatrixElementDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование EmployeeState в EmployeeStateDTO
        public async Task<IBaseResponse<EmployeeStateDTO>> CreateEmployeeStateDTO(EmployeeState state)
        {
            try
            {
                var dto = new EmployeeStateDTO()
                {
                    Id = state.Id,
                    EmployeeId = state.EmployeeId,
                    GradeId = state.GradeId,
                };

                var atributes = await _unitOfWork.EmployeeStateAttributes.GetAllAsync(x => x.EmployeeStateId == state.Id, includeProperties: new string[] {
                    "Attribute",
                });

                foreach(var attribute in atributes)
                {
                    var attributeDTO = CreateAttributeDTO(attribute);

                    if (attributeDTO.StatusCode != StatusCodes.OK || attributeDTO.Data == null)
                    {
                        return new BaseResponse<EmployeeStateDTO>()
                        {
                            Description = attributeDTO.Description,
                            StatusCode = attributeDTO.StatusCode,
                        };
                    }

                    dto.EmployeeStateAttributes.Add(attributeDTO.Data);
                }

                return new BaseResponse<EmployeeStateDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeeStateDTO>()
                {
                    Description = $"[MappingService.CreateEmployeeStateDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование EmployeeAttribute в AttributeDTO
        public IBaseResponse<AttributeDTO> CreateAttributeDTO(EmployeeAttribute attribute)
        {
            try
            {
                if (attribute.Attribute == null)
                {
                    return new BaseResponse<AttributeDTO>()
                    {
                        Description = $"[MappingService.CreateAttributeDTO] : EmployeeAttribute.Attribute is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new AttributeDTO()
                {
                    AttributeId = attribute.AttributeId,
                    Name = attribute.Attribute.Name,
                    Value = attribute.Value,
                };

                return new BaseResponse<AttributeDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AttributeDTO>()
                {
                    Description = $"[MappingService.CreateAttributeDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Преобразование EmployeeStateAttribute в AttributeDTO
        public IBaseResponse<AttributeDTO> CreateAttributeDTO(EmployeeStateAttribute attribute)
        {
            try
            {
                if (attribute.Attribute == null)
                {
                    return new BaseResponse<AttributeDTO>()
                    {
                        Description = $"[MappingService.CreateAttributeDTO] : EmployeeStateAttribute.Attribute is null",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var dto = new AttributeDTO()
                {
                    AttributeId = attribute.AttributeId,
                    Name = attribute.Attribute.Name,
                    Value = attribute.Value,
                };

                return new BaseResponse<AttributeDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AttributeDTO>()
                {
                    Description = $"[MappingService.CreateAttributeDTO] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}