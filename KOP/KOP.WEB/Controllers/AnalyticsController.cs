using KOP.BLL.Interfaces;
using KOP.Common.Enums;
using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Interfaces;
using KOP.WEB.Models.ViewModels;
using KOP.WEB.Models.ViewModels.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAnalyticsService _analyticsService;
        private readonly ISupervisorService _supervisorService;

        public AnalyticsController(IUnitOfWork unitOfWork, IAnalyticsService analyticsService, ISupervisorService supervisorService)
        {
            _unitOfWork = unitOfWork;
            _analyticsService = analyticsService;
            _supervisorService = supervisorService;
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public IActionResult GetAnalyticsLayout()
        {
            var viewModel = new AnalyticsLayoutViewModel
            {
                CurrentUserId = Convert.ToInt32(User.FindFirst(c => c.Type == "Id").Value),
            };

            return View("AnalyticsLayout", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetMarkAnalytics1()
        {
            var marks = await _unitOfWork.Marks.GetAllAsync(x => x.MarkTypeId == 3);

            var groups = marks.OrderBy(x => x.DateOfCreation).GroupBy(x => x.Period);

            var viewModel = new MarkAnalyticsViewModel();

            foreach (var group in groups)
            {
                var mark = new Mark
                {
                    SumValue = group.Sum(x => Convert.ToInt32(x.FactValue)),
                    Period = group.Key,
                };

                viewModel.Marks.Add(mark);
            }

            return View("MarkAnalytics1", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetMarkAnalytics2()
        {
            var marks = await _unitOfWork.Marks.GetAllAsync(x => x.MarkTypeId == 1);

            var groups = marks.OrderBy(x => x.DateOfCreation).GroupBy(x => x.Period);

            var viewModel = new MarkAnalyticsViewModel();

            foreach (var group in groups)
            {
                var mark = new Mark
                {
                    SumValue = group.Sum(x => Convert.ToInt32(x.FactValue)),
                    Period = group.Key,
                };

                viewModel.Marks.Add(mark);
            }

            return View("MarkAnalytics2", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetMarkAnalytics3()
        {
            var marks = await _unitOfWork.Marks.GetAllAsync(x => x.MarkTypeId == 2);

            var groups = marks.OrderBy(x => x.DateOfCreation).GroupBy(x => x.Period);

            var viewModel = new MarkAnalyticsViewModel();

            foreach (var group in groups)
            {
                var mark = new Mark
                {
                    SumValue = group.Sum(x => Convert.ToInt32(x.FactValue)),
                    Period = group.Key,
                };

                viewModel.Marks.Add(mark);
            }

            return View("MarkAnalytics3", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetMarkAnalytics4()
        {
            var marks = await _unitOfWork.Marks.GetAllAsync(x => x.MarkTypeId == 4);

            var periodGroups = marks.OrderBy(x => x.DateOfCreation).GroupBy(x => x.Period);

            var viewModel = new MarkAnalyticsViewModel();

            foreach (var periodGroup in periodGroups)
            {
                viewModel.Periods.Add(periodGroup.Key);

                var employeeGroups = periodGroup.GroupBy(x => x.EmployeeId);

                foreach(var employeeGroup in employeeGroups)
                {                  
                    if(viewModel.Employees.Any(x => x.Id == employeeGroup.Key))
                    {
                        var employee = viewModel.Employees.First(x => x.Id == employeeGroup.Key);

                        employee.Values.Add(employeeGroup.OrderByDescending(x => x.DateOfCreation).First().FactValue);
                    }
                    else
                    {
                        var employee = await _unitOfWork.Employees.GetAsync(x => x.Id == employeeGroup.Key);

                        viewModel.Employees.Add(new Employee
                        {
                            Id = employeeGroup.Key,
                            Name = employee.FullName,
                            Values = new List<int> { employeeGroup.OrderByDescending(x => x.DateOfCreation).First().FactValue },
                        });
                    }
                    
                }
            }

            return View("MarkAnalytics4", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetGradeAnalytics()
        {
            var id = Convert.ToInt32(User.FindFirstValue("Id"));

            var response = await _supervisorService.GetSubordinateEmployees(id);

            if (response.StatusCode != StatusCodes.OK || response.Data == null)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new GradeAnalyticsViewModel();

            foreach (var employee in response.Data)
            {
                var grades = await _unitOfWork.Grades.GetAllAsync(x => x.EmployeeId == employee.Id, includeProperties: new string[]
                {
                    "GradeStatus",
                });

                viewModel.gradesCount += grades.Where(x => x.GradeStatus.SystemStatus == SystemStatuses.ACCEPTED || x.GradeStatus.SystemStatus == SystemStatuses.DECLINED).Count();
                viewModel.acceptedGradesCount += grades.Where(x => x.GradeStatus.SystemStatus == SystemStatuses.ACCEPTED).Count();
                viewModel.declinedGradesCount += grades.Where(x => x.GradeStatus.SystemStatus == SystemStatuses.DECLINED).Count();
            }

            return View("GradeAnalytics", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetAssessmentAnalytics()
        {
            var id = Convert.ToInt32(User.FindFirstValue("Id"));

            var response = await _supervisorService.GetSubordinateEmployees(id);

            if (response.StatusCode != StatusCodes.OK || response.Data == null)
            {
                // Возвращаем страницу ошибки с кодом статуса и описанием
                return View("Error", new ErrorViewModel
                {
                    StatusCode = response.StatusCode,
                    Message = response.Description,
                });
            }

            var viewModel = new AssessmentAnalyticsViewModel();

            var assessmentMatrixElements = await _unitOfWork.AssessmentMatrixElements.GetAllAsync(x => x.Column == 2 && x.Row != 1);

            foreach(var element in assessmentMatrixElements)
            {
                viewModel.CompetencesNames.Add(element.Value);
            }    

            var values = new List<AssessmentResultValue>();

            foreach (var employee in response.Data)
            {
                var resutls = await _unitOfWork.AssessmentResults.GetAllAsync(x => x.JudgedId == employee.Id && x.SystemStatus == SystemStatuses.COMPLETED, includeProperties: new string[]
                {
                    "AssessmentResultValues",
                });

                foreach(var result in resutls)
                {
                    values.AddRange(result.AssessmentResultValues);
                }
            }

            var groups = values.GroupBy(x => x.AssessmentMatrixRow);

            foreach(var group in groups)
            {
                var result = (double)(group.Sum(x => x.Value)) / group.Count();

                viewModel.CompetentiesValues.Add(Math.Round(result, 1));
            }

            return View("AssessmentAnalytics", viewModel);
        }
    }
}