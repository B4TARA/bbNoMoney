using KOP.BLL.Interfaces;
using KOP.WEB.Models.ViewModels;
using KOP.WEB.Models.ViewModels.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetGradesReport()
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response = await _reportService.GetGradesReport(id);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var viewModel = new GradesReportViewModel
                {
                    Employees = response.Data,
                };

                return View("GradesReport", viewModel);
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
        public async Task<IActionResult> SaveGradesReport()
        {
            try
            {
                var id = Convert.ToInt32(User.FindFirstValue("Id"));

                var response = await _reportService.SaveGradesReport(id);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return View("Error", new ErrorViewModel
                    {
                        StatusCode = response.StatusCode,
                        Message = response.Description,
                    });
                }

                var stream = response.Data;
                var fileName = "GradesReport.xlsx";

                // Отправляем файл напрямую из потока
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
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
