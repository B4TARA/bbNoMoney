using KOP.Common.DTOs;

namespace KOP.WEB.Models.ViewModels.History
{
    public class EmployeeMarkTypeHistoryViewModel
    {
        public int EmployeeId { get; set; }
        public List<MarkDTO> Marks { get; set; } = new();
    }
}