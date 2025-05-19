using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.AssessmentEntities
{
    public class AssessmentInterval
    {
        [Key]
        public int Id { get; set; } // id интервала для качественной оценки

        [Required]
        public int AssessmentNumber { get; set; } // Номер качественной оценки

        [Required]
        public int NextAssessmentMonthInterval { get; set; } // Интервал в месяцах для следующей качественной оценки



        public AssessmentIntervalMatrix AssessmentIntervalMatrix { get; set; } // Матрица интервалов качественных оценок, к которой относится данный интервал
        public int AssessmentIntervalMatrixId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}