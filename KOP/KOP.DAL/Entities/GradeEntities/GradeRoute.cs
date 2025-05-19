using System.ComponentModel.DataAnnotations;

namespace KOP.DAL.Entities.GradeEntities
{
    public class GradeRoute
    {
        [Key]
        public int Id { get; set; } // id маршрута оценки карьерного роста



        public List<GradeRouteGroup> GradeRouteGroups { get; set; } = new(); // Порядок прохождение маршрута 



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
