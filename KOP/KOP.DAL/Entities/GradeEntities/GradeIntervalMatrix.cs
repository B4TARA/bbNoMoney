using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeIntervalMatrix
    {
        [Key]
        public int Id { get; set; } // id матрицы интервалов карьерного роста



        public List<GradeInterval> GradeIntervals { get; set; } = new(); // Интервалы между оценками, относящиеся к данной матрице интервалов
        public List<GradeType> GradeTypes { get; set; } = new(); // Типы оценок карьерного роста, которые следуют по данной матрице интервалов



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}