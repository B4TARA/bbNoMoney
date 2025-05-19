using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeResultRepository : RepositoryBase<GradeResult>, IGradeResultRepository
    {
        public GradeResultRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}