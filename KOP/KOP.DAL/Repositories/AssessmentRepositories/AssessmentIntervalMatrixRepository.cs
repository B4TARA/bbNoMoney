using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Interfaces.AssessmentInterfaces;

namespace KOP.DAL.Repositories.AssessmentRepositories
{
    public class AssessmentIntervalMatrixRepository : RepositoryBase<AssessmentIntervalMatrix>, IAssessmentIntervalMatrixRepository
    {
        public AssessmentIntervalMatrixRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}