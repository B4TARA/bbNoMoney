using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.AssessmentEntities
{
    public class AssessmentType
    {
        [Key]
        public int Id { get; set; } // id типа качественной оценки

        [Required]
        public string Name { get; set; } // название типа качественной оценки

        [Required]
        public int PlanValue { get; set; } // проходной план по всем критериям

        public int? NumberOfAssessments { get; set; } // число качественных оценок (может быть null - нет заданного количества)



        public AssessmentMatrix AssessmentMatrix { get; set; } // матрица для данного типа оценки 
        public int AssessmentMatrixId { get; set; }



        public AssessmentIntervalMatrix AssessmentIntervalMatrix { get; set; } // матрица интервалов оценок для данного типа оценки 
        public int AssessmentIntervalMatrixId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}