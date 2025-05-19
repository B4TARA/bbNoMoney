using KOP.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatusCodes = KOP.Common.Enums.StatusCodes;

namespace KOP.WEB.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetGradeTypes(int employeeId)
        {
            var response = await _gradeService.GetGradeTypes(employeeId);

            if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем сообщение об ошибке в формате JSON
                return Json(new { success = false, message = response.Description });
            }

            return Json(new { success = true, data = response.Data });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLastGradeId(int employeeId, int gradeTypeId)
        {
            var response = await _gradeService.GetLastGradeId(employeeId, gradeTypeId);

            if(response.StatusCode == StatusCodes.EntityNotFound) 
            {
                return Json(new { success = true, message = response.Description, statusCode = response.StatusCode });
            }

            else if (response.StatusCode != StatusCodes.OK)
            {
                // Возвращаем сообщение об ошибке в формате JSON
                return Json(new { success = false, message = response.Description, statusCode = response.StatusCode });
            }

            return Json(new { success = true, data = response.Data, statusCode = response.StatusCode });
        }
    }
}