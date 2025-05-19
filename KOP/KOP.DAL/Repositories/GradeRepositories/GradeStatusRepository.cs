using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeStatusRepository : RepositoryBase<GradeStatus>, IGradeStatusRepository
    {
        public GradeStatusRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
