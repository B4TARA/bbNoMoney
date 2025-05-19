using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeInterval
    {
        [Key]
        public int Id { get; set; } // id интервала для оценки карьерного роста

        [Required]
        public int GradeNumber { get; set; } // Номер оценки карьерного роста

        [Required]
        public int NextGradeMonthIntervalAccepted { get; set; } // Интервал в месяцах для следующей оценки карьерного роста в случае успешно сданной оценки

        [Required]
        public int NextGradeMonthIntervalDeclined { get; set; } // Интервал в месяцах для следующей оценки карьерного роста в случае неуспешно сданной оценки

        public GradeIntervalMatrix GradeIntervalMatrix { get; set; } // Матрица интервалов оценок карьерного роста, к которой относится данный интервал
        public int GradeIntervalMatrixId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}