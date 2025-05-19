using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeMatrix
    {
        [Key]
        public int Id { get; set; } // id матрицы карьерного роста



        public List<GradeType> GradeTypes { get; set; } = new(); // Типы оценок, к которым относится данная матрица
        public List<GradeMatrixColumn> Columns { get; set; } = new(); // Столбцы матрицы карьерного роста



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}