using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductInventoryManagement.Models;
using ProductInventoryManagement.Repositories;

namespace ProductInventoryManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _hmacKey;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _hmacKey = configuration["Security:HmacKey"];
        }

        public async Task<User> RegisterUserAsync(string username, string password, string role)
        {
            var existingUser = await _userRepository.FindAsync(u => u.Username == username);
            if (existingUser.Any())
                throw new Exception("User already exists.");

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = role
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.FindAsync(u => u.Username == username);
            if (!user.Any())
                throw new UnauthorizedAccessException("Invalid username or password.");

            var existingUser = user.First();
            if (!VerifyPassword(password, existingUser.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            return GenerateJwtToken(existingUser);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_hmacKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash).ToString();
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_hmacKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var password1 = Convert.ToBase64String(hash).ToString();
            return Convert.ToBase64String(hash) == storedHash;
        }
    }
}
