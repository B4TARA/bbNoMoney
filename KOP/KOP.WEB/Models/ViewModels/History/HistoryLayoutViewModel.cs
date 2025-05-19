using KOP.Common.DTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class HistoryLayoutViewModel
    {
        public List<EmployeeDTO> SubordinateEmployees { get; set; } = new();
    }
}