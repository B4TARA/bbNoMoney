using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Mark
    {
        [Key]
        public int Id { get; set; } // id показателя

        [Required]
        public int FactValue { get; set; } // Фактическое значение показателя

        [Required]
        public bool Result { get; set; } // Результат по показателю

        [Required]
        public string Period { get; set; } // Период показателя



        public MarkType MarkType { get; set; } // Тип показателя, к которому относится данный показатель
        public int MarkTypeId { get; set; }



        public Employee Employee { get; set; } // Сотрудник, к которому относится данный показатель
        public int EmployeeId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
