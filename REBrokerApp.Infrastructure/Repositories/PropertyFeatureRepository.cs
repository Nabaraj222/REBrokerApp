using REBrokerApp.Domain.Entities;
using REBrokerApp.Domain.Interfaces;
using REBrokerApp.Web.Data;

namespace REBrokerApp.Infrastructure.Repositories
{
    public  class PropertyFeatureRepository : RepositoryBase<PropertyFeature>, IPropertyFeatureRepository
    {
        public PropertyFeatureRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
