using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOP.DAL.Entities.GradeEntities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; } // id оценки карьерного роста

        [Required]
        public int Number { get; set; } // Номер оценки карьерного роста (очередность)

        [Required]
        public DateOnly StartDate { get; set; } // дата начала оценки карьерного роста
        public DateOnly? EndDate { get; set; } // дата завершения оценки карьерного роста
        public DateOnly? NextGradeDate { get; set; } // дата начала следующей оценки карьерного роста

        public int? CurrentGradeRouteGroupId { get; set; }


        [ForeignKey("EmployeeStateBeforeGradeId")]
        public EmployeeState EmployeeStateBeforeGrade { get; set; } // Cостояние сотрудника перед оценкой карьерного роста
        public int EmployeeStateBeforeGradeId { get; set; }


        [ForeignKey("EmployeeStateAfterGradeId")]
        public EmployeeState? EmployeeStateAfterGrade { get; set; } // Cостояние сотрудника после оценки карьерного роста
        public int? EmployeeStateAfterGradeId { get; set; }



        public Employee Employee { get; set; } // пользователь, к которому относится данная качественная оценка
        public int EmployeeId { get; set; }



        public GradeStatus GradeStatus { get; set; } // Текущий статус оценки
        public int GradeStatusId { get; set; }



        public GradeType GradeType { get; set; } // Тип оценки карьерного роста
        public int GradeTypeId { get; set; }



        public GradeRoute GradeRoute { get; set; } // Маршрут оценки конкретно для данной оценки
        public int GradeRouteId { get; set; }



        public List<GradeResult> GradeResults { get; set; } = new(); // Результаты оценки
        public List<Comment> Comments { get; set; } = new(); // Комментарии для данной оценки



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}