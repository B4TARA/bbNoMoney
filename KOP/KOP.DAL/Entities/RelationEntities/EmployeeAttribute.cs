using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.RelationEntities
{
    public class EmployeeAttribute
    {
        [Required]
        public string Value { get; set; }



        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }



        public Attribute Attribute { get; set; }
        public int AttributeId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}