using KOP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeStatus
    {
        [Key]
        public int Id { get; set; } // id статуса оценки карьерного роста

        [Required]
        public string Name { get; set; } // название статуса оценки карьерного роста

        [Required]
        public string HtmlClassName { get; set; } // имя класса для отображения цвета на html

        [Required]
        public SystemStatuses SystemStatus { get; set; } // Системный статус, к которому относится данный статус оценки



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}