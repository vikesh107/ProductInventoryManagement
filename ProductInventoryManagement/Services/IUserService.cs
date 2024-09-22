using ProductInventoryManagement.Models;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(string username, string password, string role);
        Task<string> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
