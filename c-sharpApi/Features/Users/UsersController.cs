using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace c_sharpApi.Features.Users
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new()
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}