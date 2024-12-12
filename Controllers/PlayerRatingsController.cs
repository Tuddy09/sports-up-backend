using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sports_up_backend.Database;
using sports_up_backend.Models;

namespace sports_up_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerRatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerRatingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PlayerRatings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerRating>>> GetPlayerRatings()
        {
            return await _context.PlayerRatings.ToListAsync();
        }

        // GET: api/PlayerRatings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerRating>> GetPlayerRating(int id)
        {
            var playerRating = await _context.PlayerRatings.FindAsync(id);

            if (playerRating == null)
            {
                return NotFound();
            }

            return playerRating;
        }

        // PUT: api/PlayerRatings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayerRating(int id, PlayerRating playerRating)
        {
            if (id != playerRating.RatingId)
            {
                return BadRequest();
            }

            _context.Entry(playerRating).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerRatingExists(id))
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

        // POST: api/PlayerRatings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> LeaveRating(PlayerRating playerRating)
        {
            var reviewerUser = await _context.Users.FindAsync(playerRating.RatedByUserId);
            var reviewedUser = await _context.Users.FindAsync(playerRating.RatedUserId);

            if (reviewerUser == null || reviewedUser == null)
            {
                return NotFound("One or both users not found.");
            }

            // Validate that both users participated in a finished lobby
            var lobbyPlayers = await _context.Lobbies
                .Where(l => l.LobbyPlayers.Any(lp => lp.UserId == playerRating.RatedByUserId) &&
                            l.LobbyPlayers.Any(lp => lp.UserId == playerRating.RatedUserId) &&
                            l.Status == LobbyStatus.Finished)
                .ToListAsync();

            if (!lobbyPlayers.Any())
            {
                return BadRequest("Users have not participated in a finished lobby together.");
            }

            // Add the rating
            reviewerUser.RatingsGiven.Add(playerRating);
            reviewedUser.RatingsReceived.Add(playerRating);

            _context.PlayerRatings.Add(playerRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayerRating", new { id = playerRating.RatingId }, playerRating);
        }
        
        

        // DELETE: api/PlayerRatings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerRating(int id)
        {
            var playerRating = await _context.PlayerRatings.FindAsync(id);
            if (playerRating == null)
            {
                return NotFound();
            }

            _context.PlayerRatings.Remove(playerRating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerRatingExists(int id)
        {
            return _context.PlayerRatings.Any(e => e.RatingId == id);
        }
    }
}
