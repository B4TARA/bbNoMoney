using KOP.DAL.Entities.AssessmentEntities;
using KOP.DAL.Interfaces.AssessmentInterfaces;

namespace KOP.DAL.Repositories.AssessmentRepositories
{
    public class AssessmentIntervalRepository : RepositoryBase<AssessmentInterval>, IAssessmentIntervalRepository
    {
        public AssessmentIntervalRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
