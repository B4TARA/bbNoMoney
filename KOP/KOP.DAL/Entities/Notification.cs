using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; } // id уведомления



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
