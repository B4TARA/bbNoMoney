using KOP.DAL.Entities.RelationEntities;
using KOP.DAL.Interfaces.RelationInterfaces;

namespace KOP.DAL.Repositories.RelationRepositories
{
    public class EmployeeAttributeRepository : RepositoryBase<EmployeeAttribute>, IEmployeeAttributeRepository
    {
        public EmployeeAttributeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}