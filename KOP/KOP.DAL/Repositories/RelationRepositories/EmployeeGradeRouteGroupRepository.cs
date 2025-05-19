using KOP.DAL.Entities.RelationEntities;
using KOP.DAL.Interfaces.RelationInterfaces;

namespace KOP.DAL.Repositories.RelationRepositories
{
    public class EmployeeGradeRouteGroupRepository : RepositoryBase<EmployeeGradeRouteGroup>, IEmployeeGradeRouteGroupRepository
    {
        public EmployeeGradeRouteGroupRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}