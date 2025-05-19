using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeType
    {
        [Key]
        public int Id { get; set; } // id типа оценки карьерного роста

        [Required]
        public string Name { get; set; } // название типа оценки карьерного роста

        [Required]
        public int NumberOfGrades { get; set; } // число оценок карьерного роста



        public GradeRoute GradeRoute { get; set; } // Стандартный маршрут оценки для всех оценок данного типа
        public int GradeRouteId { get; set; }



        public GradeMatrix GradeMatrix { get; set; } // матрица для данного типа оценки 
        public int GradeMatrixId { get; set; }



        public GradeIntervalMatrix GradeIntervalMatrix { get; set; } // матрица интервалов оценок для данного типа оценки 
        public int GradeIntervalMatrixId { get; set; }



        public List<Grade> Grades { get; set; } = new(); // Оценки, принадлежащие к данному типу оценок карьерного роста



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
