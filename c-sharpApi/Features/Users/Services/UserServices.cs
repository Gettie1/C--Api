using c_sharpApi.Features.Users.DTOs;
using c_sharpApi.Features.Users.Entities;
using EntitiesNs = c_sharpApi.Features.Users.Entities;
using System.Security.Cryptography;
using System.Linq;

namespace c_sharpApi.Features.Users
{
    public class UserServices
    {
    private static List<EntitiesNs.User> _users = new()
        {
            new EntitiesNs.User { Id = 1, Username = "john_doe", Email = "john@example.com", PasswordHash = "hashed_password_1" },
            new EntitiesNs.User { Id = 2, Username = "jane_doe", Email = "jane@example.com", PasswordHash = "hashed_password_2" }
        };
        public List<UserResponseDto> GetAllUsers()
        {
            return _users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
            }).ToList();
        }
        // get user by id
        public UserResponseDto? GetUserById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }
        public UserResponseDto CreateUser(CreateUserDto createUserDto)
        {
            // Hash the password before storing it
            var passwordHash = HashPassword(createUserDto.Password);

            var user = new EntitiesNs.User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = passwordHash
            };
            // Assign a new Id since this in-memory store doesn't auto-generate one.
            // Use the current max Id + 1 (or 1 when list is empty).
            var nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.Id = nextId;
            _users.Add(user);
            // Here you would typically save the user to a database
            // For this example, we'll just return a UserResponseDto

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,};
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}