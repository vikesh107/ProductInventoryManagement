using System.ComponentModel.DataAnnotations;

namespace ProductInventoryManagement.Models
{
    public class User
    {

        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }  // Hashed password
        public string Role { get; set; }  // "Admin" or "User"
    }
}
