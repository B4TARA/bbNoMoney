using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeMatrixColumn
    {
        [Key]
        public int Id { get; set; } // id столбца матрицы карьерного роста

        [Required]
        public int PassedGradeNumber { get; set; } // номер текущей сданной оценки

        [Required]
        public string NewValue { get; set; } // новое значение атрибута



        public Attribute Attribute { get; set; }
        public int AttributeId { get; set; } // id атрибута



        public GradeMatrix GradeMatrix { get; set; }
        public int GradeMatrixId { get; set; } // матрица, к которой относится текущий элемент



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}