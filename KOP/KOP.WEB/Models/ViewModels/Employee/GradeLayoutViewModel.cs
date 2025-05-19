using KOP.Common.DTOs;
using KOP.Common.DTOs.GradeDTOs;

namespace KOP.WEB.Models.ViewModels.Employee
{
    public class GradeLayoutViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeeImagePath { get; set; }

        public List<AttributeDTO> EmployeeAttributes { get; set; } = new();
        public List<GradeTypeDTO> GradeTypes { get; set; } = new();
        public List<MarkDTO> Marks { get; set; } = new();
        public List<GradeDTO> LastGrades { get; set; } = new();
    }
}