using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class MarkType
    {
        [Key]
        public int Id { get; set; } // id типа показателя

        [Required]
        public string Name { get; set; } // Название типа показателя

        [Required]
        public int PlanValue { get; set; } // Значение плана по показателю

        [Required]
        public bool IsPercentage { get; set; } // Переводится ли поле в проценты



        public List<Mark> Marks { get; set; } = new(); // Список показателей, относящихся к данному типу



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
