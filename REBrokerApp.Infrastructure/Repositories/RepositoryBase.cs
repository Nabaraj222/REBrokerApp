using Microsoft.EntityFrameworkCore;
using REBrokerApp.Domain.Interfaces;
using REBrokerApp.Web.Data;
using System.Linq.Expressions;

namespace REBrokerApp.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _dbContext;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<T>> FindAll(bool trackChanges) =>
            !trackChanges ?
             await Task.FromResult(_dbContext.Set<T>().AsNoTracking()) :
             await Task.FromResult(_dbContext.Set<T>());

        public async Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
            !trackChanges ?
              await Task.FromResult(_dbContext.Set<T>()
                .Where(expression)
                .AsNoTracking()) :
              await Task.FromResult(_dbContext.Set<T>()
                .Where(expression));

        public async Task Create(T entity) => await _dbContext.Set<T>().AddAsync(entity);

        public async Task Update(T entity)
        {
            await Task.FromResult(_dbContext.Set<T>().Update(entity));
        }

        public async Task Delete(T entity) => await Task.FromResult(_dbContext.Set<T>().Remove(entity));

        public async Task<T> GetById(int id)
        {
            return await Task.FromResult(_dbContext.Set<T>().Find(id));
        }
        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
