using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Entities.RelationEntities;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; } // id сотрудника

        [Required]
        public string FullName { get; set; } // ФИО сотрудника



        [Required]
        public string Login { get; set; } // Логин сотрудника для входа в систему

        [Required]
        public string Password { get; set; } // Пароль сотрудника для входа в систему

        [Required]
        public string Email { get; set; } // Рабочая почта сотрудника (для рассылки уведомлений и восстановления пароля)

        [Required]
        public string ImagePath { get; set; } = string.Empty; // Путь к аватарке сотрудника

        [Required]
        public bool IsSuspended { get; set; } // Флаг ограничения доступа сотрудника ко входу в систему



        public Module Module { get; set; } // Модуль сотрудника
        public int ModuleId { get; set; }



        public Role Role { get; set; } // Ролей сотрудника
        public int RoleId { get; set; }



        public List<EmployeeAttribute> EmployeeAttributes { get; set; } = new(); // Динамические атрибуты сотрудника
        public List<Mark> Marks { get; set; } = new(); // Показатели сотрудника
        public List<Grade> Grades { get; set; } = new(); // Оценки карьерного роста сотрудника
        public List<Assessment> Assessments { get; set; } = new(); // качественные оценки сотрудника
        public List<EmployeeGradeRouteGroup> EmployeeGradeRouteGroups { get; set; } = new(); // К каким группам оценки относится данный сотрудник



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}