using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeIntervalMatrixRepository : RepositoryBase<GradeIntervalMatrix>, IGradeIntervalMatrixRepository
    {
        public GradeIntervalMatrixRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}