using Microsoft.EntityFrameworkCore;
using c_sharpApi.Data;
using c_sharpApi.Features.Users.Entities;
using c_sharpApi.Features.Users.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace c_sharpApi.Features.Users.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<UserResponseDto?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto> CreateUser(CreateUserDto createUserDto)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
                throw new Exception("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
                throw new Exception("Email already exists");

            // Hash password
            var passwordHash = HashPassword(createUserDto.Password);

            // Create user
            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}