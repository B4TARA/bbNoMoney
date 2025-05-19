using KOP.Common.DTOs.GradeDTOs;

namespace KOP.Common.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
        public string Role { get; set; }

        public List<AttributeDTO> EmployeeAttributes { get; set; } = new();
        public List<MarkDTO> Marks { get; set; } = new();
        public List<GradeDTO> LastGrades { get; set; } = new();
    }
}