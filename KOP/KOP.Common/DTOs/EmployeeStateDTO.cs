namespace KOP.Common.DTOs
{
    public class EmployeeStateDTO
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int GradeId { get; set; }
        public List<AttributeDTO> EmployeeStateAttributes { get; set; } = new();
    }
}