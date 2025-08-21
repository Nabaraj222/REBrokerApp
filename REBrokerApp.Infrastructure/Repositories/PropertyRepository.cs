using Microsoft.EntityFrameworkCore.Metadata.Internal;
using REBrokerApp.Domain.Interfaces;
using REBrokerApp.Web.Data;

namespace REBrokerApp.Infrastructure.Repositories
{
    public class PropertyRepository : RepositoryBase<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
