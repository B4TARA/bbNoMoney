using KOP.DAL.Entities.RelationEntities;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Attribute
    {
        [Key]
        public int Id { get; set; } // id атрибута

        [Required]
        public string Name { get; set; } // Название атрибута



        public List<EmployeeAttribute> EmployeeAttributes { get; set; } = new(); // к каким сотрудникам относится



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}