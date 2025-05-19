using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Entities;
using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Entities.RelationEntities;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingService _mappingService;
        private readonly IGradeService _gradeService;

        public SupervisorService(IUnitOfWork unitOfWork, IMappingService mappingService, IGradeService gradeService)
        {
            _unitOfWork = unitOfWork;
            _mappingService = mappingService;
            _gradeService = gradeService;
        }

        // Получить всех сотрудников, которые нуждаются в оценке
        public async Task<IBaseResponse<List<EmployeeDTO>>> GetAppointedGrades(int supervisorId)
        {
            try
            {
                // Получаем группы оценки, в которых участвует руководитель
                var groups = await _unitOfWork.EmployeeGradeRouteGroups.GetAllAsync(x => x.EmployeeId == supervisorId);

                var gradeRouteGroupIds = groups.Select(g => g.GradeRouteGroupId).ToList();

                var grades = await _unitOfWork.Grades.GetAllAsync(x =>
                    x.CurrentGradeRouteGroupId.HasValue &&
                    gradeRouteGroupIds.Contains(x.CurrentGradeRouteGroupId.Value),
                    includeProperties: new string[]
                    {
                        "Employee.Role",
                        "GradeType",
                        "GradeStatus",
                    });

                var employees = new List<EmployeeDTO>();

                foreach (var grade in grades)
                {
                    var response = await GetEmployeeGradeNeed(supervisorId, grade.Id);

                    if (response.StatusCode != StatusCodes.OK)
                    {
                        return new BaseResponse<List<EmployeeDTO>>()
                        {
                            Description = response.Description,
                            StatusCode = response.StatusCode,
                        };
                    }

                    if (response.Data)
                    {
                        var employeeId = grade.EmployeeId;

                        // Проверяем, есть ли сотрудник в списке
                        var employee = employees.FirstOrDefault(e => e.Id == employeeId);

                        // Если нету, то добавляем в список
                        if (employee == null)
                        {
                            var employeeDTO = new EmployeeDTO()
                            {
                                Id = grade.EmployeeId,
                                FullName = grade.Employee.FullName,
                                Role = grade.Employee.Role.Name,
                            };

                            employeeDTO.LastGrades.Add(new GradeDTO
                            {
                                Id = grade.Id,
                                GradeTypeName = grade.GradeType.Name,
                                GradeStatusName = grade.GradeStatus.Name,
                                HtmlClassName = grade.GradeStatus.HtmlClassName,
                                NextGradeDate = grade.NextGradeDate,
                            });

                            employees.Add(employeeDTO);
                        }
                        // Если сотрудник уже существует, добавляем оценку в список его оценок
                        else
                        {
                            employee.LastGrades.Add(new GradeDTO
                            {
                                Id = grade.Id,
                                GradeTypeName = grade.GradeType.Name,
                                GradeStatusName = grade.GradeStatus.Name,
                                HtmlClassName = grade.GradeStatus.HtmlClassName,
                                NextGradeDate = grade.NextGradeDate,
                            });
                        }
                    }
                }

                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Data = employees,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Description = $"[SupervisorService.GetAppointedGrades] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить подчиненных сотрудников
        public async Task<IBaseResponse<List<EmployeeDTO>>> GetSubordinateEmployees(int supervisorId)
        {
            try
            {
                // Получаем руководителя по идентификатору
                var supervisor = await _unitOfWork.Employees.GetAsync(x => x.Id == supervisorId, includeProperties: new string[]
                {
                    "Module.Employees.Role",
                    "Module.Employees.Grades.GradeType",
                    "Module.Employees.Grades.GradeStatus",
                    "Module.Children.Employees.Role",
                    "Module.Children.Employees.Grades.GradeType",
                    "Module.Children.Employees.Grades.GradeStatus"
                });

                // Проверяем, существует ли в БД
                if (supervisor == null)
                {
                    return new BaseResponse<List<EmployeeDTO>>()
                    {
                        Description = $"[SupervisorService.GetAllSubordinateEmployees] : Пользователь с id = {supervisorId} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем все подчиненные роли
                var subordinateRoles = await GetSubordinateRoles(supervisor.RoleId);

                if (subordinateRoles.StatusCode != StatusCodes.OK || subordinateRoles.Data == null)
                {
                    return new BaseResponse<List<EmployeeDTO>>()
                    {
                        Description = subordinateRoles.Description,
                        StatusCode = subordinateRoles.StatusCode,
                    };
                }

                var subordinateEmployees = await GetSubordinateEmployees(supervisor.Module, subordinateRoles.Data);

                if (subordinateEmployees.StatusCode != StatusCodes.OK || subordinateEmployees.Data == null)
                {
                    return new BaseResponse<List<EmployeeDTO>>()
                    {
                        Description = subordinateEmployees.Description,
                        StatusCode = subordinateEmployees.StatusCode,
                    };
                }

                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Data = subordinateEmployees.Data,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Description = $"[SupervisorService.GetAllSubordinateEmployees] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Вспомогательный метод получения подчиненных ролей для рекурсии
        private async Task<IBaseResponse<List<RoleDTO>>> GetSubordinateRoles(int roleId)
        {
            try
            {
                // Получаем роль по идентификатору
                var role = await _unitOfWork.Roles.GetAsync(x => x.Id == roleId, includeProperties: new string[] { "Children" });

                if (role == null)
                {
                    return new BaseResponse<List<RoleDTO>>()
                    {
                        Description = $"[SupervisorService.GetSubordinateRoles] : Роль с id = {roleId} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var roleDTOs = new List<RoleDTO>();

                // Получаем дочерние роли
                foreach (var childRole in role.Children)
                {
                    roleDTOs.Add(new RoleDTO()
                    {
                        Id = childRole.Id,
                        Name = childRole.Name,
                    });

                    var subordinateRoles = await GetSubordinateRoles(childRole.Id);

                    if (subordinateRoles.StatusCode != StatusCodes.OK || subordinateRoles.Data == null)
                    {
                        return new BaseResponse<List<RoleDTO>>()
                        {
                            Description = subordinateRoles.Description,
                            StatusCode = subordinateRoles.StatusCode,
                        };
                    }

                    roleDTOs.AddRange(subordinateRoles.Data);
                }

                return new BaseResponse<List<RoleDTO>>()
                {
                    Data = roleDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<RoleDTO>>()
                {
                    Description = $"[SupervisorService.GetSubordinateRoles] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Вспомогательный метод получения подчиненных сотрудников для рекурсии
        private async Task<IBaseResponse<List<EmployeeDTO>>> GetSubordinateEmployees(Module module, List<RoleDTO> subordinateRoles)
        {
            try
            {
                var subordinateEmployees = new List<EmployeeDTO>();

                // Получаем подчиненных сотрудников текущего модуля
                foreach (var employee in module.Employees)
                {
                    if (subordinateRoles.Any(x => x.Id == employee.RoleId))
                    {
                        var employeeDTO = await _mappingService.CreateEmployeeDTO(employee);

                        if (employeeDTO.StatusCode != StatusCodes.OK || employeeDTO.Data == null)
                        {
                            continue;
                        }

                        subordinateEmployees.Add(employeeDTO.Data);
                    }
                }

                // Получаем дочерние модули
                foreach (var childModule in module.Children)
                {
                    var childSubordinateEmployees = await GetSubordinateEmployees(childModule, subordinateRoles);

                    if (childSubordinateEmployees.StatusCode != StatusCodes.OK || childSubordinateEmployees.Data == null)
                    {
                        return new BaseResponse<List<EmployeeDTO>>()
                        {
                            Description = childSubordinateEmployees.Description,
                            StatusCode = childSubordinateEmployees.StatusCode,
                        };
                    }

                    subordinateEmployees.AddRange(childSubordinateEmployees.Data);
                }

                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Data = subordinateEmployees,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Description = $"[SupervisorService.GetSubordinateEmployees] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить все подчиненные модули
        public async Task<IBaseResponse<ModuleDTO>> GetAllSubordinateModules(int supervisorId)
        {
            try
            {
                // Получаем руководителя по идентификатору
                var supervisor = await _unitOfWork.Employees.GetAsync(x => x.Id == supervisorId, includeProperties: new string[]
                {
                    "Module.Employees.Role",
                    "Module.Employees.Grades.GradeType",
                    "Module.Employees.Grades.GradeStatus",
                    "Module.Children.Employees.Role",
                    "Module.Children.Employees.Grades.GradeType",
                    "Module.Children.Employees.Grades.GradeStatus"
                });

                if (supervisor == null)
                {
                    return new BaseResponse<ModuleDTO>()
                    {
                        Description = $"[SupervisorService.GetSubordinates] : Пользователь с id = {supervisorId} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var moduleDTO = new ModuleDTO
                {
                    Id = supervisor.Module.Id,
                    Name = supervisor.Module.Name,
                    IsRoot = true,
                };

                var response = await GetSubordinateRoles(supervisor.RoleId);

                if (response.StatusCode != StatusCodes.OK)
                {
                    return new BaseResponse<ModuleDTO>()
                    {
                        Description = response.Description,
                        StatusCode = response.StatusCode,
                    };
                }

                // Получаем подчиненных сотрудников такого же модуля
                foreach (var employee in supervisor.Module.Employees)
                {
                    if (response.Data.Any(x => x.Id == employee.RoleId))
                    {
                        var employeeDTO = new EmployeeDTO()
                        {
                            Id = employee.Id,
                            FullName = employee.FullName,
                            Role = employee.Role.Name,
                        };

                        var gradeTypes = employee.Grades.GroupBy(x => x.GradeType);

                        foreach (var gradeType in gradeTypes)
                        {
                            var lastGrade = gradeType.OrderByDescending(x => x.DateOfCreation).First();

                            employeeDTO.LastGrades.Add(new GradeDTO
                            {
                                Id = lastGrade.Id,
                                GradeTypeName = gradeType.Key.Name,
                                GradeStatusName = lastGrade.GradeStatus.Name,
                                HtmlClassName = lastGrade.GradeStatus.HtmlClassName,
                                NextGradeDate = lastGrade.NextGradeDate,
                            });
                        }

                        moduleDTO.Employees.Add(employeeDTO);
                    }
                }

                foreach (var child in supervisor.Module.Children)
                {
                    var childModulesReponse = await GetSubordinateModules(child.Id);

                    if (childModulesReponse.StatusCode != StatusCodes.OK)
                    {
                        return new BaseResponse<ModuleDTO>()
                        {
                            Description = response.Description,
                            StatusCode = response.StatusCode,
                        };
                    }

                    var childModuleDTO = new ModuleDTO
                    {
                        Id = child.Id,
                        Name = child.Name,
                    };

                    foreach (var employee in child.Employees)
                    {
                        var employeeDTO = new EmployeeDTO()
                        {
                            Id = employee.Id,
                            FullName = employee.FullName,
                            Role = employee.Role.Name,
                        };

                        var gradeTypes = employee.Grades.GroupBy(x => x.GradeType);

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

                        childModuleDTO.Employees.Add(employeeDTO);
                    }

                    foreach (var childModule in childModulesReponse.Data)
                    {
                        childModuleDTO.Children.Add(childModule);
                    }

                    moduleDTO.Children.Add(childModuleDTO);
                }

                return new BaseResponse<ModuleDTO>()
                {
                    Data = moduleDTO,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ModuleDTO>()
                {
                    Description = $"[SupervisorService.GetSubordinates] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Вспомогательный метод получения дочерних модулей для рекурсии
        private async Task<IBaseResponse<List<ModuleDTO>>> GetSubordinateModules(int moduleId)
        {
            try
            {
                // Получаем модуль по идентификатору
                var module = await _unitOfWork.Modules.GetAsync(x => x.Id == moduleId, includeProperties: new string[]
                {
                    "Children.Employees.Role",
                    "Children.Employees.Grades.GradeType",
                    "Children.Employees.Grades.GradeStatus"
                });

                if (module == null)
                {
                    return new BaseResponse<List<ModuleDTO>>()
                    {
                        Description = $"[SupervisorService.GetSubordinateModules] : Модуль с id = {moduleId} не найден",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                var moduleDTOs = new List<ModuleDTO>();

                // Получаем дочерние модули
                foreach (var childModule in module.Children)
                {
                    var moduleDTO = new ModuleDTO()
                    {
                        Id = childModule.Id,
                        Name = childModule.Name,
                    };

                    foreach (var employee in childModule.Employees)
                    {
                        var employeeDTO = new EmployeeDTO()
                        {
                            Id = employee.Id,
                            FullName = employee.FullName,
                            Role = employee.Role.Name,
                        };

                        var gradeTypes = employee.Grades.GroupBy(x => x.GradeType);

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

                        moduleDTO.Employees.Add(employeeDTO);
                    }

                    var response = await GetSubordinateModules(childModule.Id);

                    if (response.StatusCode != StatusCodes.OK)
                    {
                        return new BaseResponse<List<ModuleDTO>>()
                        {
                            Description = response.Description,
                            StatusCode = response.StatusCode,
                        };
                    }

                    moduleDTO.Children.AddRange(response.Data);

                    moduleDTOs.Add(moduleDTO);
                }

                return new BaseResponse<List<ModuleDTO>>()
                {
                    Data = moduleDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ModuleDTO>>()
                {
                    Description = $"[SupervisorService.GetSubordinateRoles] : {ex.Message}",
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
                    Description = $"[SupervisorService.GetGradeType] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Получить необходимость оценки карьерного роста подчиненного сотрудника 
        public async Task<IBaseResponse<bool>> GetEmployeeGradeNeed(int supervisorId, int gradeId)
        {
            try
            {
                // Получаем оценку по идентификатору
                var grade = await _unitOfWork.Grades.GetAsync(x => x.Id == gradeId, includeProperties: new string[] { "GradeResults" });

                if (grade == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = $"[SupervisorService.GetSubordinateEmployeeGradeNeed] : Оценка с id = {gradeId} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем группу оценки по идентификатору
                var gradeRouteGroup = await _unitOfWork.GradeRouteGroups.GetAsync(x => x.Id == grade.CurrentGradeRouteGroupId, includeProperties: new string[] { "EmployeeGradeRouteGroups" });

                if (gradeRouteGroup == null)
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        StatusCode = StatusCodes.OK,
                    };
                }

                // Проходим по всем сотрудникам в группе 
                foreach (var employeeGradeRouteGroup in gradeRouteGroup.EmployeeGradeRouteGroups)
                {
                    // Если руководитель назначен и еще не оценивал в контексте данной группы и оценки
                    if (employeeGradeRouteGroup.EmployeeId == supervisorId && grade.GradeResults.FirstOrDefault(x => x.GradeRouteGroupId == gradeRouteGroup.Id && x.JudgeId == supervisorId) == null)
                    {
                        return new BaseResponse<bool>()
                        {
                            Data = true,
                            StatusCode = StatusCodes.OK,
                        };
                    }
                }

                return new BaseResponse<bool>()
                {
                    Data = false,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[SupervisorService.GetSubordinateEmployeeGradeNeed] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Одобрить переход на новый этап оценки
        public async Task<IBaseResponse<object>> AcceptEmployeeGrade(GradeEmployeeDTO gradeEmployeeDTO)
        {
            try
            {
                // Получаем текущую оценку по идентификатору
                var grade = await _unitOfWork.Grades.GetAsync(x => x.Id == gradeEmployeeDTO.GradeId, includeProperties: new string[] { "GradeResults", "GradeType", "GradeRoute", "Employee" });

                if (grade == null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = $"[SupervisorService.AcceptEmployeeGrade] : Оценка с id = {gradeEmployeeDTO.GradeId} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем текущую группу оценки для данной оценки
                var gradeRouteGroup = await _unitOfWork.GradeRouteGroups.GetAsync(x => x.Id == grade.CurrentGradeRouteGroupId, includeProperties: new string[] { "EmployeeGradeRouteGroups" });

                // Создаем новый результат оценки карьерного роста
                var newGradeResult = new GradeResult
                {
                    ResultDate = DateOnly.FromDateTime(DateTime.Today),
                    GradeResultStatus = SystemStatuses.ACCEPTED,
                    GradeRouteGroupId = gradeRouteGroup.Id,
                    GradeId = gradeEmployeeDTO.GradeId,
                    JudgeId = gradeEmployeeDTO.SupervisorId,
                    JudgedId = grade.EmployeeId,
                };

                await _unitOfWork.GradeResults.AddAsync(newGradeResult);

                // Создаем сущности комментария и обратной связи 
                if (string.IsNullOrEmpty(gradeEmployeeDTO.Comment) == false)
                {
                    await _unitOfWork.Comments.AddAsync(new Comment
                    {
                        Text = gradeEmployeeDTO.Comment,
                        IsFeedback = false,
                        EmployeeId = gradeEmployeeDTO.SupervisorId,
                        GradeId = gradeEmployeeDTO.GradeId,
                    });
                }

                if (string.IsNullOrEmpty(gradeEmployeeDTO.Feedback) == false)
                {
                    await _unitOfWork.Comments.AddAsync(new Comment
                    {
                        Text = gradeEmployeeDTO.Feedback,
                        IsFeedback = true,
                        EmployeeId = gradeEmployeeDTO.SupervisorId,
                        GradeId = gradeEmployeeDTO.GradeId,
                    });
                };

                // Получаем оценки от данной группы
                var gradeResults = grade.GradeResults.Where(x => x.GradeRouteGroupId == gradeRouteGroup.Id);

                // Получаем количество одобрений от данной группы (учитываем текущую одобренную оценку)
                var acceptancesNumber = gradeResults.Where(x => x.GradeResultStatus == SystemStatuses.ACCEPTED).Count();

                // Если хватает одобрений для продолжения 
                if (acceptancesNumber >= gradeRouteGroup.RequiredAcceptancesNumber)
                {
                    // Получаем маршрут для текущей оценки
                    var gradeRoute = await _unitOfWork.GradeRoutes.GetAsync(x => x.Id == grade.GradeRoute.Id, includeProperties: new string[] { "GradeRouteGroups" });

                    // Получаем последнюю группу в маршруте
                    var lastGradeRouteGroup = gradeRoute.GradeRouteGroups.OrderByDescending(x => x.QueueNumber).FirstOrDefault();

                    // Если текущая группа является последней в маршруте
                    if (lastGradeRouteGroup.Id == gradeRouteGroup.Id)
                    {
                        // Получаем статус, соответсвующий успешно завершенной оценке
                        var acceptedGradeStatus = await _unitOfWork.GradeStatuses.GetAsync(x => x.SystemStatus == SystemStatuses.ACCEPTED);

                        if (acceptedGradeStatus == null)
                        {
                            // Отменяем все изменения в БД
                            await _unitOfWork.RollbackAsync();

                            return new BaseResponse<object>()
                            {
                                Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует статуса оценки, соответствующего системному статусу ACCEPTED",
                                StatusCode = StatusCodes.EntityNotFound,
                            };
                        }

                        // Если текущая оценка не была последней оценкой данного типа
                        if (grade.GradeType.NumberOfGrades != grade.Number)
                        {
                            // Получаем матрицу интервалов для данного типа оценки карьерного роста
                            var gradeIntervalMatrix = await _unitOfWork.GradeIntervalMatrices.GetAsync(x => x.Id == grade.GradeType.GradeIntervalMatrixId, includeProperties: new string[] { "GradeIntervals" });

                            if (gradeIntervalMatrix == null)
                            {
                                // Отменяем все изменения в БД
                                await _unitOfWork.RollbackAsync();

                                return new BaseResponse<object>()
                                {
                                    Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует матрицы интервалов для типа оценки с id = {grade.GradeType.Id}",
                                    StatusCode = StatusCodes.EntityNotFound,
                                };
                            }

                            // Получаем интервал для следующей оценки
                            var gradeInterval = gradeIntervalMatrix.GradeIntervals.FirstOrDefault(x => x.GradeNumber == grade.Number);

                            if (gradeInterval == null)
                            {
                                // Отменяем все изменения в БД
                                await _unitOfWork.RollbackAsync();

                                return new BaseResponse<object>()
                                {
                                    Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует интервала для матрицы интервалов с id = {gradeIntervalMatrix.Id} и оценки с номером = {grade.Number}",
                                    StatusCode = StatusCodes.EntityNotFound,
                                };
                            }

                            grade.NextGradeDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(gradeInterval.NextGradeMonthIntervalAccepted);
                        }
                        // Если текущая оценка была последней оценкой данного типа
                        else
                        {
                            grade.NextGradeDate = null;
                        }

                        // Получаем матрицу карьерного роста для данного типа оценки
                        var gradeMatrix = await _unitOfWork.GradeMatrices.GetAsync(x => x.Id == grade.GradeType.GradeMatrixId, includeProperties: new string[] { "Columns.Attribute" });

                        if (gradeMatrix == null)
                        {
                            // Отменяем все изменения в БД
                            await _unitOfWork.RollbackAsync();

                            return new BaseResponse<object>()
                            {
                                Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует матрицы карьерного роста для типа оценки с id = {grade.GradeType.Id}",
                                StatusCode = StatusCodes.EntityNotFound,
                            };
                        }

                        // Получаем колонки, соответствующие пройденной оценке
                        var gradeMatrixСolumns = gradeMatrix.Columns.Where(x => x.PassedGradeNumber == grade.Number);

                        // Получаем все значения атрибутов сотрудника
                        var employeeAttributes = await _unitOfWork.EmployeeAttributes.GetAllAsync(x => x.EmployeeId == grade.EmployeeId);

                        // Создаем объект состояния сотрудника после оценки
                        var employeeStateAfterGrade = new EmployeeState
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
                            employeeStateAfterGrade.EmployeeStateAttributes.Add(new EmployeeStateAttribute
                            {
                                Value = employeeAttribute.Value,
                                AttributeId = employeeAttribute.AttributeId,
                            });
                        }

                        // Сохраняем состояния сотрудника
                        //await _unitOfWork.EmployeeStates.AddAsync(employeeStateAfterGrade);

                        // Успешно завершаем оценку
                        grade.EndDate = DateOnly.FromDateTime(DateTime.Today);
                        grade.EmployeeStateAfterGrade = employeeStateAfterGrade;
                        grade.CurrentGradeRouteGroupId = null;
                        grade.GradeStatusId = acceptedGradeStatus.Id;
                    }
                    // Если нет, то переходим на следующую группу оценки
                    else
                    {
                        // Получаем следующую группу оценки в маршруте
                        var nextGradeRouteGroup = gradeRoute.GradeRouteGroups.FirstOrDefault(x => x.QueueNumber == gradeRouteGroup.QueueNumber + 1);

                        if (nextGradeRouteGroup == null)
                        {
                            // Отменяем все изменения в БД
                            await _unitOfWork.RollbackAsync();

                            return new BaseResponse<object>()
                            {
                                Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно перейти на следующую группу оценки, так как для оценки с id = {gradeEmployeeDTO.GradeId} не существует группы оценки с порядковым номером = {gradeRouteGroup.QueueNumber + 1}",
                                StatusCode = StatusCodes.EntityNotFound,
                            };
                        }

                        // Меняем статус текущий оценки на следующий
                        grade.GradeStatusId = nextGradeRouteGroup.GradeStatusId;
                        grade.CurrentGradeRouteGroupId = nextGradeRouteGroup.Id;

                        _unitOfWork.Grades.Update(grade);
                    }
                }
                // Если не хватает, но оценщик - последний в группе оценки (учитывая текущий результат)
                else if (gradeResults.Count() == gradeRouteGroup.EmployeeGradeRouteGroups.Count())
                {
                    // Получаем статус, соответствующий неуспешно завершенной оценке
                    var declinedGradeStatus = await _unitOfWork.GradeStatuses.GetAsync(x => x.SystemStatus == SystemStatuses.DECLINED);

                    if (declinedGradeStatus == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует статуса оценки, соответствующего системному статусу DECLINED",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    // Получаем матрицу интервалов для данного типа оценки карьерного роста
                    var gradeIntervalMatrix = await _unitOfWork.GradeIntervalMatrices.GetAsync(x => x.Id == grade.GradeType.GradeIntervalMatrixId, includeProperties: new string[] { "GradeIntervals" });

                    if (gradeIntervalMatrix == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует матрицы интервалов для типа оценки с id = {grade.GradeType.Id}",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    // Получаем интервал, соотвутсвующий неуспешно сданной оценке
                    var gradeInterval = gradeIntervalMatrix.GradeIntervals.FirstOrDefault(x => x.GradeNumber == grade.Number);

                    if (gradeInterval == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.AcceptEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует интервала для матрицы интервалов с id = {gradeIntervalMatrix.Id} и оценки с номером = {grade.Number}",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    grade.NextGradeDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(gradeInterval.NextGradeMonthIntervalDeclined);

                    // Получаем все значения атрибутов сотрудника
                    var employeeAttributes = await _unitOfWork.EmployeeAttributes.GetAllAsync(x => x.EmployeeId == grade.EmployeeId);

                    // Создаем объект состояния сотрудника после оценки
                    var employeeStateAfterGrade = new EmployeeState
                    {
                        EmployeeId = grade.EmployeeId,
                        GradeId = grade.Id,
                    };

                    // Записываем значение каждого атрибута в состояние сотрудника после оценки
                    foreach (var employeeAttribute in employeeAttributes)
                    {
                        employeeStateAfterGrade.EmployeeStateAttributes.Add(new EmployeeStateAttribute
                        {
                            Value = employeeAttribute.Value,
                            AttributeId = employeeAttribute.AttributeId,
                        });
                    }

                    // Сохраняем состояние сотрудника после оценки
                    await _unitOfWork.EmployeeStates.AddAsync(employeeStateAfterGrade);

                    // Неуспешно завершаем оценку
                    grade.EndDate = DateOnly.FromDateTime(DateTime.Today);
                    grade.EmployeeStateAfterGrade = employeeStateAfterGrade;
                    grade.CurrentGradeRouteGroupId = null;
                    grade.GradeStatusId = declinedGradeStatus.Id;
                }

                // Сохраняем все изменения в БД
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                // Отменяем все изменения в БД
                await _unitOfWork.RollbackAsync();

                return new BaseResponse<object>()
                {
                    Description = $"[SupervisorService.AcceptEmployeeGrade] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Отклонить переход на новый этап оценки
        public async Task<IBaseResponse<object>> DeclineEmployeeGrade(GradeEmployeeDTO gradeEmployeeDTO)
        {
            try
            {
                // Получаем текущую оценку по идентификатору
                var grade = await _unitOfWork.Grades.GetAsync(x => x.Id == gradeEmployeeDTO.GradeId, includeProperties: new string[]
                {
                    "GradeResults",
                    "GradeType",
                    "GradeRoute",
                    "Employee"
                });

                if (grade == null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = $"[SupervisorService.DeclineEmployeeGrade] : Оценка с id = {gradeEmployeeDTO.GradeId} не найдена",
                        StatusCode = StatusCodes.EntityNotFound,
                    };
                }

                // Получаем текущую группу оценки для данной оценки
                var gradeRouteGroup = await _unitOfWork.GradeRouteGroups.GetAsync(x => x.Id == grade.CurrentGradeRouteGroupId, includeProperties: new string[]
                {
                    "EmployeeGradeRouteGroups"
                });

                // Создаем новый результат оценки карьерного роста
                var newGradeResult = new GradeResult
                {
                    ResultDate = DateOnly.FromDateTime(DateTime.Today),
                    GradeResultStatus = SystemStatuses.DECLINED,
                    GradeRouteGroupId = gradeRouteGroup.Id,
                    GradeId = gradeEmployeeDTO.GradeId,
                    JudgeId = gradeEmployeeDTO.SupervisorId,
                    JudgedId = grade.EmployeeId,
                };

                await _unitOfWork.GradeResults.AddAsync(newGradeResult);

                // Создаем сущности комментария и обратной связи 
                if (string.IsNullOrEmpty(gradeEmployeeDTO.Comment) == false)
                {
                    await _unitOfWork.Comments.AddAsync(new Comment
                    {
                        Text = gradeEmployeeDTO.Comment,
                        IsFeedback = false,
                        EmployeeId = gradeEmployeeDTO.SupervisorId,
                        GradeId = gradeEmployeeDTO.GradeId,
                    });
                }

                if (string.IsNullOrEmpty(gradeEmployeeDTO.Feedback) == false)
                {
                    await _unitOfWork.Comments.AddAsync(new Comment
                    {
                        Text = gradeEmployeeDTO.Feedback,
                        IsFeedback = true,
                        EmployeeId = gradeEmployeeDTO.SupervisorId,
                        GradeId = gradeEmployeeDTO.GradeId,
                    });
                };

                // Получаем оценки от данной группы
                var gradeResults = grade.GradeResults.Where(x => x.GradeRouteGroupId == gradeRouteGroup.Id);

                // Получаем количество одобрений от данной группы (учитываем текущую одобренную оценку)
                var acceptancesNumber = gradeResults.Where(x => x.GradeResultStatus == SystemStatuses.ACCEPTED).Count();

                // Если оценщик - последний в группе оценки
                if (gradeResults.Count() == gradeRouteGroup.EmployeeGradeRouteGroups.Count())
                {
                    // Получаем статус, соответсвующий неуспешно завершенной оценке
                    var declinedGradeStatus = await _unitOfWork.GradeStatuses.GetAsync(x => x.SystemStatus == SystemStatuses.DECLINED);

                    if (declinedGradeStatus == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.DeclineEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует статуса оценки, соответствующего системному статусу Declined",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    // Получаем матрицу интервалов для данного типа оценки карьерного роста
                    var gradeIntervalMatrix = await _unitOfWork.GradeIntervalMatrices.GetAsync(x => x.Id == grade.GradeType.GradeIntervalMatrixId, includeProperties: new string[] { "GradeIntervals" });

                    if (gradeIntervalMatrix == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.DeclineEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует матрицы интервалов для типа оценки с id = {grade.GradeType.Id}",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    // Получаем интервал, соотвутствующий неуспешно сданной оценке
                    var gradeInterval = gradeIntervalMatrix.GradeIntervals.FirstOrDefault(x => x.GradeNumber == grade.Number);

                    if (gradeInterval == null)
                    {
                        // Отменяем все изменения в БД
                        await _unitOfWork.RollbackAsync();

                        return new BaseResponse<object>()
                        {
                            Description = $"[SupervisorService.DeclineEmployeeGrade] : Невозможно завершить оценку с id = {gradeEmployeeDTO.GradeId}, так как в БД не существует интервала для матрицы интервалов с id = {gradeIntervalMatrix.Id} и оценки с номером = {grade.Number}",
                            StatusCode = StatusCodes.EntityNotFound,
                        };
                    }

                    grade.NextGradeDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(gradeInterval.NextGradeMonthIntervalDeclined);

                    // Получаем все значения атрибутов сотрудника
                    var employeeAttributes = await _unitOfWork.EmployeeAttributes.GetAllAsync(x => x.EmployeeId == grade.EmployeeId);

                    // Создаем объект состояния сотрудника после оценки
                    var employeeStateAfterGrade = new EmployeeState
                    {
                        EmployeeId = grade.EmployeeId,
                        GradeId = grade.Id,
                    };

                    // Записываем значение каждого атрибута в состояние сотрудника после оценки
                    foreach (var employeeAttribute in employeeAttributes)
                    {
                        employeeStateAfterGrade.EmployeeStateAttributes.Add(new EmployeeStateAttribute
                        {
                            Value = employeeAttribute.Value,
                            AttributeId = employeeAttribute.AttributeId,
                        });
                    }

                    // Сохраняем состояние сотрудника после оценки
                    await _unitOfWork.EmployeeStates.AddAsync(employeeStateAfterGrade);

                    // Неуспешно завершаем оценку
                    grade.EndDate = DateOnly.FromDateTime(DateTime.Today);
                    grade.EmployeeStateAfterGrade = employeeStateAfterGrade;
                    grade.CurrentGradeRouteGroupId = null;
                    grade.GradeStatusId = declinedGradeStatus.Id;
                }

                // Сохраняем все изменения в БД
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                // Отменяем все изменения в БД
                await _unitOfWork.RollbackAsync();

                return new BaseResponse<object>()
                {
                    Description = $"[SupervisorService.DeclineEmployeeGrade] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}