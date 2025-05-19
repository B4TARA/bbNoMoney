using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeMatrixRepository : RepositoryBase<GradeMatrix>, IGradeMatrixRepository
    {
        public GradeMatrixRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
