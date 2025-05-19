using KOP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeResult
    {
        [Key]
        public int Id { get; set; } // id результата оценки карьерного роста оценщиком

        [Required]
        public DateOnly ResultDate { get; set; } // дата оценки карьерного роста оценщиком

        [Required]
        public SystemStatuses GradeResultStatus { get; set; } // Текущий статус оценки карьерного роста



        public GradeRouteGroup GradeRouteGroup { get; set; } // группа оценки, к которой относится данный результат оценки 
        public int GradeRouteGroupId { get; set; }



        public Grade Grade { get; set; } // оценка, к которой относится данный результат оценки 
        public int GradeId { get; set; }



        public Employee Judge { get; set; } // оценщик
        public int JudgeId { get; set; }



        public Employee Judged { get; set; } // оцениваемый
        public int JudgedId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}