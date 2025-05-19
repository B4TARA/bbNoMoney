using KOP.BLL.Interfaces;
using KOP.WEB.Models.ViewModels;
using KOP.WEB.Models.ViewModels.History;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ISupervisorService _supervisorService;
        private readonly IEmployeeService _employeeService;
        private readonly IMarkService _markService;
        private readonly IGradeService _gradeService;
        private readonly IAssessmentService _assessmentService;

        public HistoryController(ISupervisorService supervisorService, IEmployeeService employeeService, IMarkService markService, IGradeService gradeService, IAssessmentService assessmentService)
        {
            _supervisorService = supervisorService;
            _employeeService = employeeService;
            _markService = markService;
            _gradeService = gradeService;
            _assessmentService = assessmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetHistoryLayout()
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response = await _supervisorService.GetSubordinateEmployees(id);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new HistoryLayoutViewModel
                {
                    SubordinateEmployees = response.Data,
                };

                return View("HistoryLayout", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeHistoryLayout(int employeeId)
        {
            try
            {
                var response1 = await _employeeService.GetEmployeeName(employeeId);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response1.StatusCode,
                        Message = response1.Description,
                    });
                }

                var response2 = await _markService.GetMarkTypes(employeeId);

                if (response2.StatusCode != StatusCodes.OK || response2.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response2.StatusCode,
                        Message = response2.Description,
                    });
                }

                var viewModel = new EmployeeHistoryLayoutViewModel
                {
                    EmployeeId = employeeId,
                    EmployeeFullName = response1.Data,
                    MarkTypes = response2.Data,
                };

                return View("EmployeeHistoryLayout", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeGradeHistoryLayout(int employeeId)
        {
            try
            {
                var response = await _gradeService.GetGradeTypes(employeeId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeGradeHistoryLayoutViewModel
                {
                    EmployeeId = employeeId,
                    GradeTypes = response.Data,
                };

                return View("EmployeeGradeHistoryLayout", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeAssessmentHistoryLayout(int employeeId)
        {
            try
            {
                var response = await _assessmentService.GetAssessmentTypes(employeeId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeAssessmentHistoryLayoutViewModel
                {
                    EmployeeId = employeeId,
                    AssessmentTypes = response.Data,
                };

                return View("EmployeeAssessmentHistoryLayout", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeMarkTypeHistory(int employeeId, int markTypeId)
        {
            try
            {
                var response = await _markService.GetMarks(employeeId, markTypeId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeMarkTypeHistoryViewModel
                {
                    EmployeeId = employeeId,
                    Marks = response.Data,
                };

                return View("EmployeeMarkTypeHistory", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeGradeTypeHistory(int employeeId, int gradeTypeId)
        {
            try
            {
                var response1 = await _gradeService.GetGrades(employeeId, gradeTypeId);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response1.StatusCode,
                        Message = response1.Description,
                    });
                }

                // Тут получаем информацию о сотруднкие + вложенные свойства (что не надо для текущей задачи)
                var response2 = await _employeeService.GetEmployee(employeeId);

                if (response2.StatusCode != StatusCodes.OK || response2.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response2.StatusCode,
                        Message = response2.Description,
                    });
                }

                var viewModel = new EmployeeGradeTypeHistoryViewModel
                {
                    EmployeeId = employeeId,
                    EmployeFullName = response2.Data.FullName,
                    EmployeeImagePath = response2.Data.ImagePath,
                    Grades = response1.Data,
                };

                return View("EmployeeGradeTypeHistory", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetEmployeeAssessmentTypeHistory(int employeeId, int assessmentTypeId)
        {
            try
            {
                var response = await _assessmentService.GetAssessmentType(employeeId, assessmentTypeId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeAssessmentTypeHistoryViewModel
                {
                    EmployeeId = employeeId,
                    AssessmentTypePlanValue = response.Data.PlanValue,
                    Assessments = response.Data.Assessments,
                };

                return View("EmployeeAssessmentTypeHistory", viewModel);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}