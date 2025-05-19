using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeIntervalRepository : RepositoryBase<GradeInterval>, IGradeIntervalRepository
    {
        public GradeIntervalRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
