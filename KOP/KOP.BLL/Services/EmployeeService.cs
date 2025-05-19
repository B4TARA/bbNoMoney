using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Entities;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessmentService;
        private readonly IGradeService _gradeService;

        public EmployeeService(IUnitOfWork unitOfWork, IAssessmentService assessmentService, IGradeService gradeService)
        {
            _unitOfWork = unitOfWork;
            _assessmentService = assessmentService;
            _gradeService = gradeService;
        }

        // Получить информацию о сотруднике по id
        public async Task<IBaseResponse<EmployeeDTO>> GetEmployee(int id)
        {
            try
            {
                // Получаем сотрудника по идентификатору
                var employee = await _unitOfWork.Employees.GetAsync(x => x.Id == id, includeProperties: new string[]
                {
                    "Marks.MarkType",
                    "EmployeeAttributes.Attribute",
                    "Grades.GradeType",
                    "Grades.GradeStatus",
                });

                if (employee == null)
                {
                    return new BaseResponse<EmployeeDTO>()
                    {
                        Description = $"[EmployeeService.GetEmployee] : Пользователь с id = {id} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var employeeDTO = new EmployeeDTO
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    ImagePath = employee.ImagePath,
                };

                // Группируем массив количественных оценок по "Тип оценки"
                var gradeTypes = employee.Grades.GroupBy(x => x.GradeType);

                // Достаем последнюю количественную оценку для каждого типа (группы)
                foreach (var gradeType in gradeTypes)
                {
                    var lastGrade = gradeType.OrderByDescending(x => x.DateOfCreation).FirstOrDefault();

                    employeeDTO.LastGrades.Add(new GradeDTO
                    {
                        Id = lastGrade.Id,
                        GradeTypeName = gradeType.Key.Name,
                        GradeStatusName = lastGrade.GradeStatus.Name,
                        HtmlClassName = lastGrade.GradeStatus.HtmlClassName,
                        NextGradeDate = lastGrade.NextGradeDate,
                    });
                }

                // Группируем массив показателей по "Тип показателя"
                var markTypes = employee.Marks.GroupBy(x => x.MarkType);

                // Достаем последний показатель для каждого типа (группы)
                foreach (var markType in markTypes)
                {
                    var lastMark = markType.OrderByDescending(x => x.DateOfCreation).First();

                    var dto = new MarkDTO
                    {
                        Id = lastMark.Id,
                        TypeName = markType.Key.Name,
                        PlanValue = markType.Key.PlanValue,
                        FactValue = lastMark.FactValue,
                        Result = lastMark.Result,
                        IsPercentage = lastMark.MarkType.IsPercentage,
                        Period = lastMark.Period,
                    };

                    if (dto.IsPercentage)
                    {
                        dto.PercentageValue = Convert.ToInt32(Math.Round((double)dto.FactValue / dto.PlanValue, 2) * 100);
                    }

                    employeeDTO.Marks.Add(dto);                 
                }

                // Получаем значения атрабутов сотрудника
                var employeeAttributes = employee.EmployeeAttributes;

                // Записываем каждый атрибут в DTO
                foreach (var employeeAttribute in employeeAttributes)
                {
                    employeeDTO.EmployeeAttributes.Add(new AttributeDTO
                    {
                        AttributeId = employeeAttribute.AttributeId,
                        Name = employeeAttribute.Attribute.Name,
                        Value = employeeAttribute.Value,
                    });
                }

                return new BaseResponse<EmployeeDTO>()
                {
                    Data = employeeDTO,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeeDTO>()
                {
                    Description = $"[EmployeeService.GetEmployee] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить состояние сотрудника в случае успешного прохождения оценки
        public async Task<BaseResponse<EmployeeStateDTO>> GetEmployeeStateAfterGrade(int gradeId)
        {
            try
            {
                // Получаем текущую оценку по идентификатору
                var grade = await _unitOfWork.Grades.GetAsync(x => x.Id == gradeId, includeProperties: new string[] { "GradeResults", "GradeType", "GradeRoute", "Employee" });

                if (grade == null)
                {
                    return new BaseResponse<EmployeeStateDTO>()
                    {
                        Description = $"[SupervisorService.AcceptEmployeeGrade] : Оценка с id = {gradeId} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем матрицу карьерного роста для данного типа оценки
                var gradeMatrix = await _unitOfWork.GradeMatrices.GetAsync(x => x.Id == grade.GradeType.GradeMatrixId, includeProperties: new string[] { "Columns.Attribute" });

                if (gradeMatrix == null)
                {
                    return new BaseResponse<EmployeeStateDTO>()
                    {
                        Description = $"[SupervisorService.AcceptEmployeeGrade] : Не существует матрицы карьерного роста для типа оценки с id = {grade.GradeType.Id}",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем колонки, соответствующие пройденной оценке
                var gradeMatrixСolumns = gradeMatrix.Columns.Where(x => x.PassedGradeNumber == grade.Number);

                // Получаем все значения атрибутов сотрудника
                var employeeAttributes = await _unitOfWork.EmployeeAttributes.GetAllAsync(x => x.EmployeeId == grade.EmployeeId);

                // Создаем объект состояния сотрудника после оценки
                var employeeStateAfterGrade = new EmployeeStateDTO
                {
                    EmployeeId = grade.Employee.Id,
                    GradeId = grade.Id,
                };

                foreach (var gradeMatrixСolumn in gradeMatrixСolumns)
                {
                    // Ищем атрибут сотрудника, который должен измениться по матрице перехода
                    var employeeAttribute = employeeAttributes.FirstOrDefault(x => x.AttributeId == gradeMatrixСolumn.AttributeId);

                    // Если не находим, то пропускаем
                    if (employeeAttribute == null)
                    {
                        continue;
                    }

                    // Если находим, то меняем значение на новое по матрице
                    employeeAttribute.Value = gradeMatrixСolumn.NewValue;

                    _unitOfWork.EmployeeAttributes.Update(employeeAttribute);

                    // Записываем значение атрибута в объект состояния сотрудника после оценки
                    employeeStateAfterGrade.EmployeeStateAttributes.Add(new AttributeDTO
                    {
                        Value = employeeAttribute.Value,
                        AttributeId = employeeAttribute.AttributeId,
                    });
                }

                return new BaseResponse<EmployeeStateDTO>()
                {
                    Data = employeeStateAfterGrade,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<EmployeeStateDTO>()
                {
                    Description = $"[EmployeeService.GetEmployeeStateAfterGrade] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить информацию о типе количественных оценок определенного сотрудника
        public async Task<IBaseResponse<List<GradeDTO>>> GetGradeType(int employeeId, int gradeTypeId)
        {
            try
            {
                var response1 = await _gradeService.GetGrades(employeeId, gradeTypeId);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return new BaseResponse<List<GradeDTO>>()
                    {
                        StatusCode = response1.StatusCode,
                        Description = response1.Description,
                    };
                }

                var response2 = await _gradeService.GetGradeType(gradeTypeId);

                if (response2.StatusCode != StatusCodes.OK || response2.Data == null)
                {
                    return new BaseResponse<List<GradeDTO>>()
                    {
                        StatusCode = response2.StatusCode,
                        Description = response2.Description,
                    };
                }

                var gradeDTOs = new List<GradeDTO>();

                // Группируем все оценки по номеру
                var groups = response1.Data.GroupBy(x => x.Number);

                foreach (var group in groups)
                {
                    // Выбираем самую последнюю оценку такого номера
                    var gradeDTO = group.OrderByDescending(x => x.DateOfCreation).First();

                    gradeDTOs.Add(gradeDTO);
                }

                for (int i = gradeDTOs.Count(); i < response2.Data.NumberOfGrades; i++)
                {
                    gradeDTOs.Add(new GradeDTO
                    {
                        Number = i + 1,
                        IsClickable = false,
                    });
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
                    Description = $"[EmployeeService.GetGradeType] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить имя сотрудника по id
        public async Task<IBaseResponse<string>> GetEmployeeName(int employeeId)
        {
            try
            {
                // Получаем сотрудника по идентификатору
                var employee = await _unitOfWork.Employees.GetAsync(x => x.Id == employeeId);

                if (employee == null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = $"[EmployeeService.GetEmployeeName] : Пользователь с id = {employeeId} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                return new BaseResponse<string>()
                {
                    Data = employee.FullName,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[EmployeeService.GetEmployeeName] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить дочерние роли
        public async Task<IBaseResponse<bool>> HasChildRoles(int roleId)
        {
            // Получаем сотрудника по идентификатору
            var role = await _unitOfWork.Roles.GetAsync(x => x.Id == roleId, includeProperties: new string[] { "Children" });

            if (role == null)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[EmployeeService.GetChildRoles] : Роль с id = {roleId} не найдена",
                    StatusCode = StatusCodes.EntityNotFound,
                };
            }

            if (role.Children.Count() == 0)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCodes.OK,
                };
            }
            else
            {
                return new BaseResponse<bool>()
                {
                    Data = true,
                    StatusCode = StatusCodes.OK,
                };
            }
        }




        // Получить последнюю качественную оценку каждого типа по id сотрудника
        public async Task<IBaseResponse<List<AssessmentDTO>>> GetEmployeeLastAssessments(int employeeId, int supervisorId)
        {
            try
            {
                // Получаем сотрудника по идентификатору
                var employee = await _unitOfWork.Employees.GetAsync(x => x.Id == employeeId, includeProperties: new string[] { "Assessments.AssessmentType" });

                if (employee == null)
                {
                    return new BaseResponse<List<AssessmentDTO>>()
                    {
                        Description = $"[EmployeeService.GetEmployeeLastAssessments] : Пользователь с id = {employeeId} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Группируем массив оценок по "Типу оценок"
                var assessmentsByAssessmentType = employee.Assessments.GroupBy(x => x.AssessmentType);

                var lastAssessmentDTOs = new List<AssessmentDTO>();

                // Достаем последнюю оценку для каждого типа (группы)
                foreach (var assessmentGroup in assessmentsByAssessmentType)
                {
                    var lastAssessment = assessmentGroup.OrderByDescending(x => x.DateOfCreation).First();

                    var lastAssessmentDTO = new AssessmentDTO
                    {
                        Id = lastAssessment.Id,
                        EmployeeId = employeeId,
                        Number = lastAssessment.Number,
                        AssessmentTypeName = assessmentGroup.Key.Name,
                    };

                    var response = await _assessmentService.IsActiveAssessment(supervisorId, employeeId, lastAssessment.Id);

                    if (response.StatusCode != StatusCodes.OK)
                    {
                        return new BaseResponse<List<AssessmentDTO>>()
                        {
                            Description = response.Description,
                            StatusCode = response.StatusCode,
                        };
                    }

                    lastAssessmentDTO.IsActiveAssessment = response.Data;

                    lastAssessmentDTOs.Add(lastAssessmentDTO);
                }

                return new BaseResponse<List<AssessmentDTO>>()
                {
                    Data = lastAssessmentDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AssessmentDTO>>()
                {
                    Description = $"[EmployeeService.GetEmployeeLastAssessments] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }





        // Получить объект качественной самооценки
        public async Task<IBaseResponse<AssessmentResultDTO>> GetSelfAssessment(int employeeId, int assessmentId)
        {
            try
            {
                var dto = new AssessmentResultDTO();

                // Получаем объект самооценки по assessmentId
                var assessment = await _unitOfWork.Assessments.GetAsync(x => x.Id == assessmentId);

                // Получаем тип качественной оценки, к которому относится данная оценка
                var assessmentType = await _unitOfWork.AssessmentTypes.GetAsync(x => x.Id == assessment.AssessmentTypeId);

                //Получаем результат самооценки
                var selfResult = await _unitOfWork.AssessmentResults.GetAsync(x => x.JudgedId == employeeId && x.JudgeId == employeeId && x.AssessmentId == assessmentId, includeProperties: new string[]
                {
                    "AssessmentResultValues"
                });

                if (selfResult == null)
                {
                    dto.SystemStatus = SystemStatuses.NOT_EXIST;

                    return new BaseResponse<AssessmentResultDTO>()
                    {
                        Data = dto,
                        StatusCode = StatusCodes.OK
                    };
                }
                else if (selfResult.SystemStatus == SystemStatuses.COMPLETED)
                {
                    // Получаем значения результатов самооценки
                    var selfValues = selfResult.AssessmentResultValues;

                    foreach (var selfValue in selfValues)
                    {
                        dto.Values.Add(new AssessmentResultValueDTO
                        {
                            Value = selfValue.Value,
                            AssessmentMatrixRow = selfValue.AssessmentMatrixRow,
                        });

                        // Создаем нужное количество объектов для последующего заполения результатами оценщиков
                        dto.AverageValues.Add(new AssessmentResultValueDTO
                        {
                            Value = 0,
                            AssessmentMatrixRow = selfValue.AssessmentMatrixRow,
                        });
                    }

                    // Получаем результаты оценки по всем оценщикам, кроме самооценки
                    var results = await _unitOfWork.AssessmentResults.GetAllAsync(x => x.JudgedId == employeeId && x.AssessmentId == assessmentId, includeProperties: new string[]
                    {
                        "AssessmentResultValues"
                    });

                    foreach (var result in results)
                    {
                        // Получаем значения результатов оценщика
                        var values = result.AssessmentResultValues;

                        foreach (var value in values)
                        {
                            dto.AverageValues.First(x => x.AssessmentMatrixRow == value.AssessmentMatrixRow).Value += value.Value;
                        }
                    }

                    // Делим на количество оценщиков для получения среднего результата
                    foreach (var averageValue in dto.AverageValues)
                    {
                        var sumValue = dto.AverageValues.First(x => x.AssessmentMatrixRow == averageValue.AssessmentMatrixRow).Value;

                        var result = Math.Round((double)sumValue / results.Count(), 1);

                        dto.AverageValues.First(x => x.AssessmentMatrixRow == averageValue.AssessmentMatrixRow).Value = result;

                        dto.AverageResult += result;

                        dto.PlanValue = assessmentType.PlanValue;
                    }
                }

                dto.Judge = new EmployeeDTO
                {
                    Id = employeeId,
                };

                dto.Judged = new EmployeeDTO
                {
                    Id = employeeId,
                };

                dto.Id = selfResult.Id;
                dto.AssessmentId = selfResult.AssessmentId;
                dto.SystemStatus = selfResult.SystemStatus;

                // Получаем матрицу качественной оценки, которая относится к данному типу
                var assessmentMatrix = await _unitOfWork.AssessmentMatrices.GetAsync(x => x.Id == assessmentType.AssessmentMatrixId, includeProperties: new string[]
                {
                    "Elements"
                });

                dto.MinValue = assessmentMatrix.MinAssessmentMatrixResultValue;
                dto.MaxValue = assessmentMatrix.MaxAssessmentMatrixResultValue;

                var dtos = new List<AssessmentMatrixElementDTO>();

                foreach (var element in assessmentMatrix.Elements)
                {
                    dtos.Add(new AssessmentMatrixElementDTO
                    {
                        Row = element.Row,
                        Value = element.Value,
                    });
                }

                dto.Elements = dtos;

                dto.ElementsByRow = dto.Elements.GroupBy(x => x.Row).ToList();

                return new BaseResponse<AssessmentResultDTO>()
                {
                    Data = dto,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AssessmentResultDTO>()
                {
                    Description = $"[EmployeeService.GetSelfAssessment] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить коллег сотрудника для качественной оценки
        public async Task<IBaseResponse<List<AssessmentResultDTO>>> GetColleagueAssessmentResults(int employeeId)
        {
            try
            {
                //Получаем назначеные результаты оценок коропративных компетенций
                var assessmentResults = await _unitOfWork.AssessmentResults.GetAllAsync(x => x.JudgeId == employeeId && x.JudgedId != employeeId && x.SystemStatus == SystemStatuses.PENDING);

                var dtos = new List<AssessmentResultDTO>();

                // Получаем каждого оцениваемого коллегу по идентификатору
                foreach (var assessmentResult in assessmentResults)
                {
                    // Получаем объект оценки по assessmentId
                    var assessment = await _unitOfWork.Assessments.GetAsync(x => x.Id == assessmentResult.AssessmentId, includeProperties: new string[]
                    {
                        "AssessmentResults.Judged",
                        "AssessmentResults.AssessmentResultValues",
                        "AssessmentType.AssessmentMatrix.Elements"
                    });

                    var dto = new AssessmentResultDTO
                    {
                        Id = assessmentResult.Id,
                        AssessmentId = assessmentResult.AssessmentId,
                        SystemStatus = assessmentResult.SystemStatus,
                        Sum = assessmentResult.AssessmentResultValues.Sum(x => x.Value),
                        TypeName = assessment.AssessmentType.Name,
                        MinValue = assessment.AssessmentType.AssessmentMatrix.MinAssessmentMatrixResultValue,
                        MaxValue = assessment.AssessmentType.AssessmentMatrix.MaxAssessmentMatrixResultValue,
                    };

                    dto.Judge = new EmployeeDTO
                    {
                        Id = employeeId,
                    };

                    dto.Judged = new EmployeeDTO
                    {
                        Id = assessment.EmployeeId,
                        ImagePath = assessmentResult.Judged.ImagePath,
                        FullName = assessmentResult.Judged.FullName,
                    };

                    foreach (var value in assessmentResult.AssessmentResultValues)
                    {
                        dto.Values.Add(new AssessmentResultValueDTO
                        {
                            Value = value.Value,
                            AssessmentMatrixRow = value.AssessmentMatrixRow,
                        });
                    }

                    foreach (var element in assessment.AssessmentType.AssessmentMatrix.Elements)
                    {
                        dto.Elements.Add(new AssessmentMatrixElementDTO
                        {
                            Row = element.Row,
                            Value = element.Value,
                        });
                    }

                    dto.ElementsByRow = dto.Elements.GroupBy(x => x.Row).ToList();

                    dtos.Add(dto);
                }

                return new BaseResponse<List<AssessmentResultDTO>>()
                {
                    Data = dtos,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AssessmentResultDTO>>()
                {
                    Description = $"[EmployeeService.GetColleagueAssessmentResults] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }



        // Оценить сотрудника (в том числе и самооценка)
        public async Task<IBaseResponse<object>> AssessEmployee(AssessEmployeeDTO assessEmployeeDTO)
        {
            try
            {
                // Находим объект оценки по id
                var assessmentResult = await _unitOfWork.AssessmentResults.GetAsync(x => x.Id == assessEmployeeDTO.AssessmentResultId);

                assessmentResult.ResultDate = DateOnly.FromDateTime(DateTime.Today);

                assessmentResult.SystemStatus = SystemStatuses.COMPLETED;

                // Создаем объекты результатов оценки
                for (int i = 0; i < assessEmployeeDTO.ResultValues.Count(); i++)
                {
                    assessmentResult.AssessmentResultValues.Add(new DAL.Entities.AssessmentEntities.AssessmentResultValue
                    {
                        Value = Convert.ToInt32(assessEmployeeDTO.ResultValues[i]),
                        AssessmentMatrixRow = (i + 1),
                    });
                }

                _unitOfWork.AssessmentResults.Update(assessmentResult);

                // Проверка на условия завершения оценки
                // ПОМЕНЯТЬ ЭТОТ КОСТЫЛЬ !!! (если уже есть в БД оценка одна (руководителя или самооценка), которая завершена
                var secondAssessmentResult = await _unitOfWork.AssessmentResults.GetAsync(x => x.AssessmentId == assessmentResult.AssessmentId && x.SystemStatus == SystemStatuses.COMPLETED);

                if (secondAssessmentResult == null)
                {
                    await _unitOfWork.CommitAsync();

                    return new BaseResponse<object>()
                    {
                        StatusCode = StatusCodes.OK
                    };
                }
                else
                {
                    var assessment = await _unitOfWork.Assessments.GetAsync(x => x.Id == assessmentResult.AssessmentId, includeProperties: new string[]
                    {
                        "AssessmentType",
                    });

                    var assessmentIntervalMatrix = await _unitOfWork.AssessmentIntervalMatrices.GetAsync(x => x.Id == assessment.AssessmentType.AssessmentIntervalMatrixId, includeProperties: new string[]
                    {
                        "AssessmentIntervals",
                    });

                    var interval = assessmentIntervalMatrix.AssessmentIntervals.FirstOrDefault(x => x.AssessmentNumber == assessment.Number);

                    assessment.EndDate = DateOnly.FromDateTime(DateTime.Today);
                    assessment.NextAssessmentDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(interval.NextAssessmentMonthInterval);
                    assessment.SystemStatus = SystemStatuses.COMPLETED;

                    _unitOfWork.Assessments.Update(assessment);

                    await _unitOfWork.CommitAsync();
                }

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
                // // // // // // // // // // //
            }
            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[EmployeeService.AssessEmployee] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}