using REBrokerApp.Domain.Entities;
using REBrokerApp.Domain.Interfaces;
using REBrokerApp.Web.Data;

namespace REBrokerApp.Infrastructure.Repositories
{
    public class PropertyImageRepository : RepositoryBase<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}