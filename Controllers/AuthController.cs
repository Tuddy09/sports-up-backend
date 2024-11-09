using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using sports_up_backend.Database;
using sports_up_backend.Models;

namespace sports_up_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email already in use.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(LoginRequest user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (dbUser == null)
            {
                return BadRequest("Invalid email.");
            }

            if (dbUser.Password != user.Password)
            {
                return BadRequest("Invalid password.");
            }

            return Ok(dbUser);
        }

    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
