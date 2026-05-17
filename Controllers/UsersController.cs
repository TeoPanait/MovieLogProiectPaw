using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieLog.Data;
using MovieLog.Models;
using MovieLog.DTOs;

namespace MovieLog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role) // Includem relația cu tabelul de Roluri
                .Select(u => new UserDto(
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role.Name // Extragem doar numele rolului
                ))
                .ToListAsync();

            return Ok(users);
        }

        // GET: /api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            return Ok(new UserDto(user.Id, user.Username, user.Email, user.Role.Name));
        }

        // POST: /api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, 
                RoleId = dto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id },
                new UserDto(user.Id, user.Username, user.Email, "User"));
        }
    }
}