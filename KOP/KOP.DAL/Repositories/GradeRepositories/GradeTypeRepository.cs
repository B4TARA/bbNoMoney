using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeTypeRepository : RepositoryBase<GradeType>, IGradeTypeRepository
    {
        public GradeTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
