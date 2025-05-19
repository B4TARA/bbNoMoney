using KOP.BLL.Interfaces;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.WEB.Models.RequestModels;
using KOP.WEB.Models.ViewModels;
using KOP.WEB.Models.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGradeService _gradeService;
        private readonly IAssessmentService _assessmentService;

        public EmployeeController(IEmployeeService employeeService, IGradeService gradeService, IAssessmentService assessmentService)
        {
            _employeeService = employeeService;
            _gradeService = gradeService;
            _assessmentService = assessmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult GetEmployeeLayout()
        {
            var currentUserId = Convert.ToInt32(User.FindFirstValue("Id"));

            return View("EmployeeLayout", currentUserId);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetGrade(int id)
        {
            var response = await _gradeService.GetGrade(id);

            if (response.StatusCode != StatusCodes.OK || response.Data == null)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new GradeViewModel
            {
                Grade = response.Data,
            };

            return View("Grade", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAssessmentLayout(int employeeId)
        {
            var response = await _assessmentService.IsActiveAssessment(employeeId, employeeId);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new AssessmentLayoutViewModel
            {
                EmployeeId = employeeId,
                IsActiveSelfAssessment = response.Data
            };

            return View("AssessmentLayout", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetGradeLayout(int employeeId)
        {
            var getEmployeeResponse = await _employeeService.GetEmployee(employeeId);

            if (getEmployeeResponse.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = getEmployeeResponse.StatusCode,
                    Message = getEmployeeResponse.Description,
                });
            }

            var getEmployeeGradeTypesResponse = await _gradeService.GetGradeTypes(employeeId);

            if (getEmployeeGradeTypesResponse.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = getEmployeeGradeTypesResponse.StatusCode,
                    Message = getEmployeeGradeTypesResponse.Description,
                });
            }

            var viewModel = new GradeLayoutViewModel
            {
                EmployeeId = getEmployeeResponse.Data.Id,
                EmployeeFullName = getEmployeeResponse.Data.FullName,
                EmployeeImagePath = getEmployeeResponse.Data.ImagePath,
                EmployeeAttributes = getEmployeeResponse.Data.EmployeeAttributes,
                LastGrades = getEmployeeResponse.Data.LastGrades,
                GradeTypes = getEmployeeGradeTypesResponse.Data,
                Marks = getEmployeeResponse.Data.Marks,
            };

            return View("GradeLayout", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetGradeType(int employeeId, int gradeTypeId)
        {
            var getGradeTypeResponse = await _employeeService.GetGradeType(employeeId, gradeTypeId);

            if (getGradeTypeResponse.StatusCode != StatusCodes.OK)
            {
                return View("Error", new ErrorViewModel
                {
                    StatusCode = getGradeTypeResponse.StatusCode,
                    Message = getGradeTypeResponse.Description,
                });
            }

            var viewModel = new GradeTypeViewModel
            { 
                Grades = getGradeTypeResponse.Data
            };

            return View("GradeType", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetColleaguesAssessment(int employeeId)
        {
            var response = await _employeeService.GetColleagueAssessmentResults(employeeId);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new ColleagueAssessmentViewModel
            {
                ColleagueAssessmentResults = response.Data,
            };

            return View("ColleaguesAssessment", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetSelfAssessment(int employeeId, int assessmentId)
        {
            var response = await _employeeService.GetSelfAssessment(employeeId, assessmentId);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new SelfAssessmentViewModel
            {
                SelfAssessmentResult = response.Data,
            };

            return View("SelfAssessment", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetSelfAssessmentLayout(int employeeId)
        {
            var response = await _employeeService.GetEmployeeLastAssessments(employeeId, employeeId);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new SelfAssessmentLayoutViewModel
            {
                LastAssessments = response.Data,
            };

            return View("SelfAssessmentLayout", viewModel);
        }      

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AssessEmployee([FromBody] AssessEmployeeRequestModel requestModel)
        {
            var assessEmployeeDTO = new AssessEmployeeDTO
            {
                ResultValues = requestModel.resultValues,
                AssessmentResultId = requestModel.assessmentResultId,
            };

            var response = await _employeeService.AssessEmployee(assessEmployeeDTO);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем ответ со статусом
                return StatusCode(Convert.ToInt32(response.StatusCode), new
                {
                    message = response.Description
                });
            }

            return StatusCode(200);
        }
    }
}
