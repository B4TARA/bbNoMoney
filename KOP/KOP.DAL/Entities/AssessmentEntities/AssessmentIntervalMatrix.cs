using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.AssessmentEntities
{
    public class AssessmentIntervalMatrix
    {
        [Key]
        public int Id { get; set; } // id матрицы интервалов карьерного роста



        public List<AssessmentInterval> AssessmentIntervals { get; set; } = new(); // Интервалы между качественными оценками, относящиеся к данной матрице интервалов
        public List<AssessmentType> AssessmentTypes { get; set; } = new(); // Типы качественных оценок, которые следуют по данной матрице интервалов



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}