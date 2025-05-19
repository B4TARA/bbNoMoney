using ClosedXML.Excel;
using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupervisorService _supervisorService;

        public ReportService(IUnitOfWork unitOfWork, ISupervisorService supervisorService)
        {
            _unitOfWork = unitOfWork;
            _supervisorService = supervisorService;
        }

        // Получить отчет по сотрудникам
        public async Task<IBaseResponse<List<EmployeeDTO>>> GetGradesReport(int supervisorId)
        {
            try
            {
                var response = await _supervisorService.GetSubordinateEmployees(supervisorId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return new BaseResponse<List<EmployeeDTO>>()
                    {
                        Description = response.Description,
                        StatusCode = response.StatusCode,
                    };
                }

                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Data = response.Data,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<EmployeeDTO>>()
                {
                    Description = $"[ReportService.GetGradesReport] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }

        // Сохранить отчет по сотрудникам
        public async Task<IBaseResponse<MemoryStream>> SaveGradesReport(int supervisorId)
        {
            try
            {
                var response = await GetGradesReport(supervisorId);

                if (response.StatusCode != StatusCodes.OK || response.Data == null)
                {
                    return new BaseResponse<MemoryStream>()
                    {
                        Description = response.Description,
                        StatusCode = response.StatusCode,
                    };
                }

                var workbook = new XLWorkbook();
                workbook.AddWorksheet("sheetName");
                var ws = workbook.Worksheet("sheetName");

                int row = 1;

                ws.Cell("A" + row.ToString()).Value = "ФИО";
                ws.Cell("A" + row.ToString()).Style.Font.Bold = true;

                ws.Cell("B" + row.ToString()).Value = "Роль";
                ws.Cell("B" + row.ToString()).Style.Font.Bold = true;

                ws.Cell("C" + row.ToString()).Value = "Тип количественной оценки";
                ws.Cell("C" + row.ToString()).Style.Font.Bold = true;

                ws.Cell("D" + row.ToString()).Value = "Результат количественной оценки";
                ws.Cell("D" + row.ToString()).Style.Font.Bold = true;

                row++;

                foreach (var record in response.Data)
                {
                    foreach(var grade in record.LastGrades)
                    {
                        ws.Cell("A" + row.ToString()).Value = record.FullName;

                        ws.Cell("B" + row.ToString()).Value = record.Role;

                        ws.Cell("C" + row.ToString()).Value = grade.GradeTypeName;

                        ws.Cell("D" + row.ToString()).Value = grade.GradeStatusName;

                        row++;
                    }                 
                }

                var stream = new MemoryStream();

                workbook.SaveAs(stream);

                stream.Position = 0; // Устанавливаем указатель в начало потока

                return new BaseResponse<MemoryStream>()
                {
                    Data = stream,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MemoryStream>()
                {
                    Description = $"[ReportService.SaveGradesReport] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}