using ProductInventoryManagement.Models;
using System.Linq.Expressions;

namespace ProductInventoryManagement.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);

        IQueryable<T> GetAll();
        Task RemoveRangeAsync(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<Product> products);
    }
}
