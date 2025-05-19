using KOP.DAL.Entities.RelationEntities;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class EmployeeState
    {
        [Key]
        public int Id { get; set; } // id состояния сотрудника

        public int EmployeeId { get; set; } // id сотрудника

        public int GradeId { get; set; } // id сотрудника



        public List<EmployeeStateAttribute> EmployeeStateAttributes { get; set; } = new(); // Набор динамических атрибутов для данного состояния сотрудника



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
