using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.RelationEntities
{
    public class EmployeeStateAttribute
    {
        [Required]
        public string Value { get; set; }



        public EmployeeState EmployeeState { get; set; }
        public int EmployeeStateId { get; set; }



        public Attribute Attribute { get; set; }
        public int AttributeId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}