using KOP.DAL.Interfaces;
using Attribute = KOP.DAL.Entities.Attribute;

namespace KOP.DAL.Repositories
{
    public class AttributeRepository : RepositoryBase<Attribute>, IAttributeRepository
    {
        public AttributeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}