using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeRouteRepository : RepositoryBase<GradeRoute>, IGradeRouteRepository
    {
        public GradeRouteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
