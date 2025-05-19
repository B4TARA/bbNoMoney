using KOP.Common.DTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeHistoryLayoutViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public List<MarkTypeDTO> MarkTypes { get; set; } = new();
    }
}