using c_sharpApi.Features.Users.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace c_sharpApi.Features.Users
{
    // public class User
    // {
    //     public int Id { get; set; }
    //     public string FirstName { get; set; } = string.Empty;
    //     public string LastName { get; set; } = string.Empty;
    //     public string Email { get; set; } = string.Empty;
    // }

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
         private readonly UserServices _userServices;

        public UsersController(UserServices userServices)
        {
            _userServices = userServices;
        }
        // private static List<User> _users = new()
        // {
        //     new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
        //     new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        // };

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userServices.GetAllUsers());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userServices.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpPost]
        public IActionResult CreateUser(CreateUserDto createUserDto)
        {
            if (string.IsNullOrEmpty(createUserDto.Username))
            {
                return BadRequest("Username is required.");
            }
            if (string.IsNullOrEmpty(createUserDto.Email))
            {
                return BadRequest("Email is required.");
            }

            var existingUser = _userServices.GetAllUsers().FirstOrDefault(u => u.Email == createUserDto.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var newUser = _userServices.CreateUser(createUserDto);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
    }
}   