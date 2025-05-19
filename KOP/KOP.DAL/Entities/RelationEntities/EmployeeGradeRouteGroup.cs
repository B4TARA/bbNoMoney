using KOP.DAL.Entities.GradeEntities;

namespace KOP.DAL.Entities.RelationEntities
{
    public class EmployeeGradeRouteGroup
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }



        public GradeRouteGroup GradeRouteGroup { get; set; }
        public int GradeRouteGroupId { get; set; }



        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}