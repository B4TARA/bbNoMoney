using KOP.DAL.Entities.GradeEntities;
using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; } // id комментария

        [Required]
        public string Text { get; set; } // текст комментария

        [Required]
        public bool IsFeedback { get; set; } // является ли комментарий обратной связью для сотрудника


        public Employee Employee { get; set; } // Сотрудник, оставивший комментарий
        public int EmployeeId { get; set; }



        public Grade Grade { get; set; } // Оценка, к которой оставлен данный комментарий
        public int GradeId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
