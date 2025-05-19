using KOP.Common.DTOs;

namespace KOP.WEB.Models.ViewModels.Report
{
    public class GradesReportViewModel
    {
        public List<EmployeeDTO> Employees { get; set; } = new List<EmployeeDTO>();
    }
}