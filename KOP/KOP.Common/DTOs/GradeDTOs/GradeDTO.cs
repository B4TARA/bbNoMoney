using KOP.Common.Enums;

namespace KOP.Common.DTOs.GradeDTOs
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int GradeTypeId { get; set; }
        public int EmployeeId { get; set; }
        public int Number { get; set; }
        public string GradeTypeName { get; set; }
        public string GradeStatusName { get; set; }
        public string HtmlClassName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? NextGradeDate { get; set; }
        public List<CommentDTO> Comments { get; set; } = new();
        public bool IsClickable { get; set; }
        public SystemStatuses SystemStatus { get; set; }

        public EmployeeStateDTO? EmployeeStateBeforeGrade { get; set; }
        public EmployeeStateDTO? EmployeeStateAfterGrade { get; set; }

        public DateOnly DateOfCreation { get; set; }
    }
}