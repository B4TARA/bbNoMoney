using KOP.DAL.Entities;
using KOP.DAL.Interfaces;

namespace KOP.DAL.Repositories
{
    public class EmployeeStateRepository : RepositoryBase<EmployeeState>, IEmployeeStateRepository
    {
        public EmployeeStateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
