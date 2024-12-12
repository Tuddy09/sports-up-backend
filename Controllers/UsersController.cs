using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sports_up_backend.Data_Transfer_Obejcts;
using sports_up_backend.Database;
using sports_up_backend.Models;

namespace sports_up_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        //GET: api/Users/getUsersFromLobby
        [HttpGet("lobbyusers/{lobbyId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersFromLobby(int lobbyId)
        {
            //We will get only users from a finished lobby, because only those are rateable users
            var lobbies = await _context.Lobbies
                .AnyAsync(l => l.Status == LobbyStatus.Finished 
                               && l.LobbyId == lobbyId);
            if (!lobbies)
            {
                return NotFound("Lobby not found or is not finished");
            }
            
            var users = await _context.LobbyPlayers
                .Where(lp => lp.LobbyId == lobbyId)
                .Select(lp => lp.User)
                .ToListAsync();
            
            return users.Count > 0 ? users : NotFound();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            var lobbyPlayers = _context.LobbyPlayers.Where(x => x.UserId == user.UserId);
            _context.LobbyPlayers.RemoveRange(lobbyPlayers);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        [HttpGet("Profile/{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user ID");
                }

                var user = await _context.Users
                    .Include(u => u.RatingsReceived)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                var userFinishedMatches = await _context.Lobbies
                    .Where(l =>
                    l.LobbyPlayers.Any(lp => lp.UserId == id) &&
                    l.Status == LobbyStatus.Finished)
                    .ToListAsync();

                var mostFrequentSport = userFinishedMatches
                    .GroupBy(l => l.Sport)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault() ?? "None";

                var averageRating = user.RatingsReceived.Any()? user.RatingsReceived.Average(rating => rating.Stars): 0;

                averageRating = averageRating - Math.Floor(averageRating) > 0.5 ? Math.Ceiling(averageRating) : Math.Floor(averageRating);


                return Ok(new UserProfileDTO
                {
                    Username = user.Username,
                    Age = user.Age,
                    AvatarId = user.AvatarId,
                    TotalMatchesPlayed = userFinishedMatches.Count(),
                    PreferredSport = mostFrequentSport,
                    Rating = (int)averageRating
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred");
            }
        }
    }
}
