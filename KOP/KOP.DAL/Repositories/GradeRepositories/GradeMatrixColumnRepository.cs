using KOP.DAL.Entities.GradeEntities;
using KOP.DAL.Interfaces.GradeInterfaces;

namespace KOP.DAL.Repositories.GradeRepositories
{
    public class GradeMatrixColumnRepository : RepositoryBase<GradeMatrixColumn>, IGradeMatrixColumnRepository
    {
        public GradeMatrixColumnRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
