using ProductInventoryManagement.Models;
using System.Linq.Expressions;

namespace ProductInventoryManagement.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate);
    }
}
