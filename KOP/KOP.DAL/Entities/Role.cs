using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; } // id роли

        [Required]
        public string Name { get; set; } // Название роли



        public virtual Role Parent { get; set; } // Родительская роль
        public int? ParentId { get; set; }



        public virtual List<Role> Children { get; set; } = new(); // Дочерние роли
        public List<Employee> Employees { get; set; } = new(); // Сотрудники модуля
        //public SystemRoles SystemRole { get; set; } = new(); // Системная роль для текущей роли



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
