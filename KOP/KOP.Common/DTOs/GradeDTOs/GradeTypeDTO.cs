namespace KOP.Common.DTOs.GradeDTOs
{
    public class GradeTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public int NumberOfGrades { get; set; }

        public List<GradeDTO> Grades { get; set; } = new();
        public List<GradeStatusDTO> GradeStatuses { get; set; } = new();
    }
}

public class GradeStatusDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HtmlClassName { get; set; }
}