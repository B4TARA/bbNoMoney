using KOP.BLL.Interfaces;
using KOP.Common.DTOs.GradeDTOs;
using KOP.Common.Enums;
using KOP.WEB.Models.RequestModels;
using KOP.WEB.Models.ViewModels;
using KOP.WEB.Models.ViewModels.Supervisor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ISupervisorService _supervisorService;
        private readonly IAssessmentService _assessmentService;
        private readonly IGradeService _gradeService;
        private readonly IEmployeeService _employeeService;
        private readonly IMarkService _markService;

        public SupervisorController(
            ISupervisorService supervisorService, 
            IAssessmentService assessmentService,
            IGradeService gradeService,
            IEmployeeService employeeService,
            IMarkService markService)
        {
            _supervisorService = supervisorService;
            _assessmentService = assessmentService;
            _gradeService = gradeService;
            _employeeService = employeeService;
            _markService = markService;
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public IActionResult GetSupervisorLayout()
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                return View("SupervisorLayout", id);
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
        public IActionResult GetAppointedGradesLayout()
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                return View("AppointedGradesLayout", id);
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
        public async Task<IActionResult> GetAppointedGrades(int supervisorId)
        {
            try
            {
                var response = await _supervisorService.GetAppointedGrades(supervisorId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new AppointedGradesLayoutViewModel
                {
                    Employees = response.Data,
                };

                return View("AppointedGrades", viewModel);
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
        public async Task<IActionResult> GetSubordinates(int supervisorId)
        {
            try
            {
                var response = await _supervisorService.GetAllSubordinateModules(supervisorId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new SubordinatesViewModel
                {
                    Module = response.Data,
                };

                return View("Subordinates", viewModel);
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
        public async Task<IActionResult> GetEmployeeLayout(int employeeId)
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response = await _assessmentService.IsActiveAssessment(id, employeeId);

                if (response.StatusCode != StatusCodes.OK)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeViewModel
                {
                    EmployeeId = employeeId,
                    IsActiveAssessment = response.Data,
                };

                return View("EmployeeLayout", viewModel);
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
        public async Task<IActionResult> GetEmployeeGradeLayout(int employeeId)
        {
            try
            {
                var response1 = await _employeeService.GetEmployee(employeeId);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response1.StatusCode,
                        Message = response1.Description,
                    });
                }

                var response2 = await _gradeService.GetGradeTypes(employeeId);

                if (response2.StatusCode != StatusCodes.OK)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response2.StatusCode,
                        Message = response2.Description,
                    });
                }

                var viewModel = new EmployeeGradeLayoutViewModel
                {
                    EmployeeId = employeeId,
                    EmployeeFullName = response1.Data.FullName,
                    EmployeeImagePath = response1.Data.ImagePath,
                    EmployeeAttributes = response1.Data.EmployeeAttributes,
                    LastGrades = response1.Data.LastGrades,
                    GradeTypes = response2.Data,
                    Marks = response1.Data.Marks,
                };

                return View("EmployeeGradeLayout", viewModel);
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
        public async Task<IActionResult> GetEmployeeAssessmentLayout(int employeeId)
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response = await _employeeService.GetEmployeeLastAssessments(employeeId, id);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeAssessmentLayoutViewModel
                {
                    LastAssessments = response.Data,
                };

                return View("EmployeeAssessmentLayout", viewModel);
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
        public async Task<IActionResult> GetEmployeeAssessment(int employeeId)
        {
            try
            {
                var viewModel = new EmployeeAssessmentViewModel();

                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response1 = await _assessmentService.GetAssessment(employeeId, SystemStatuses.COMPLETED);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response1.StatusCode,
                        Message = response1.Description,
                    });
                }

                viewModel.Assessment = response1.Data;

                var response2 = await _assessmentService.GetAssessmentResult(id, employeeId);

                if (response2.StatusCode == StatusCodes.OK && response2.Data != null)
                {
                    viewModel.SupervisorAssessmentResult = response2.Data;
                }

                return View("EmployeeAssessment", viewModel);
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
        public async Task<IActionResult> GetEmployeeGrade(int gradeId)
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response1 = await _gradeService.GetGrade(gradeId);

                if (response1.StatusCode != StatusCodes.OK || response1.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response1.StatusCode,
                        Message = response1.Description,
                    });
                }

                var response2 = await _supervisorService.GetEmployeeGradeNeed(id, gradeId);

                if (response2.StatusCode != StatusCodes.OK)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response2.StatusCode,
                        Message = response2.Description,
                    });
                }

                var response3 = await _employeeService.GetEmployeeStateAfterGrade(gradeId);

                if (response3.StatusCode != StatusCodes.OK || response3.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response3.StatusCode,
                        Message = response3.Description,
                    });
                }

                var viewModel = new EmployeeGradeViewModel
                {
                    SupervisorId = id,
                    Grade = response1.Data,
                    IsNeedSupervisorGrade = response2.Data,
                    EmployeeStateAfterGrade = response3.Data,
                };

                return View("EmployeeGrade", viewModel);
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
        public async Task<IActionResult> GetEmployeeGradeType(int employeeId, int gradeTypeId)
        {
            try
            {
                var response = await _supervisorService.GetGradeType(employeeId, gradeTypeId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new EmployeeGradeTypeViewModel
                {
                    Grades = response.Data,
                };

                return View("EmployeeGradeType", viewModel);
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

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> AcceptEmployeeGrade([FromBody] GradeEmployeeRequestModel requestModel)
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var gradeEmployeeDTO = new GradeEmployeeDTO
                {
                    SupervisorId = id,
                    GradeId = requestModel.gradeId,
                    Comment = requestModel.comment,
                    Feedback = requestModel.feedback,
                };

                var response = await _supervisorService.AcceptEmployeeGrade(gradeEmployeeDTO);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return StatusCode(Convert.ToInt32(response.StatusCode), new
                    {
                        message = response.Description
                    });
                }

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(Convert.ToInt32(StatusCodes.InternalServerError), new
                {
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeclineEmployeeGrade([FromBody] GradeEmployeeRequestModel requestModel)
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var gradeEmployeeDTO = new GradeEmployeeDTO
                {
                    SupervisorId = id,
                    GradeId = requestModel.gradeId,
                    Comment = requestModel.comment,
                    Feedback = requestModel.feedback,
                };

                var response = await _supervisorService.DeclineEmployeeGrade(gradeEmployeeDTO);

                if (response.StatusCode != StatusCodes.OK)
                {
                    return StatusCode(Convert.ToInt32(response.StatusCode), new
                    {
                        message = response.Description
                    });
                }

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(Convert.ToInt32(StatusCodes.InternalServerError), new
                {
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> SetMarkValue([FromBody] SetMarkValueRequestModel requestModel)
        {
            try
            {
                var response = await _markService.SetMarkValue(requestModel.markId, Convert.ToInt32(requestModel.markValue));

                if (response.StatusCode != StatusCodes.OK)
                {
                    return StatusCode(Convert.ToInt32(response.StatusCode), new
                    {
                        message = response.Description
                    });
                }

                return StatusCode(200);
            }
            catch
            {
                return StatusCode(Convert.ToInt32(StatusCodes.InternalServerError), new
                {
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}