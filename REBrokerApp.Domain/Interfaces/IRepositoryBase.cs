using System.Linq.Expressions;

namespace REBrokerApp.Domain.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAll(bool trackChanges = false);
        Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
        Task<T> GetById(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task SaveChanges();
    }
}
