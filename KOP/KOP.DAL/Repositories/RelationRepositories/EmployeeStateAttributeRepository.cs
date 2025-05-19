using KOP.DAL.Entities.RelationEntities;
using KOP.DAL.Interfaces.RelationInterfaces;

namespace KOP.DAL.Repositories.RelationRepositories
{
    public class EmployeeStateAttributeRepository : RepositoryBase<EmployeeStateAttribute>, IEmployeeStateAttributeRepository
    {
        public EmployeeStateAttributeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}