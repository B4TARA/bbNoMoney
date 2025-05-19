using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeRouteGroupRepository : RepositoryBase<GradeRouteGroup>, IGradeRouteGroupRepository
    {
        public GradeRouteGroupRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
