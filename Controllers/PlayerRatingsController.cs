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
            var reviewer = _context.LobbyPlayers.FirstOrDefault(
                lp => lp.UserId == playerRating.RatedByUserId);
            var reviewed = _context.LobbyPlayers.FirstOrDefault(
                lp => lp.UserId == playerRating.RatedByUserId);
            
            if (reviewer == null || reviewed == null)
            {
                return NotFound();
            }

            var lobbyPlayers = await _context.Lobbies
                .Where(l => l.LobbyPlayers.Contains(reviewer) &&
                            l.LobbyPlayers.Contains(reviewed) &&
                            l.Status == LobbyStatus.Finished)
                .ToListAsync();


            if (lobbyPlayers.Any())
            {
                return NotFound();
            }
            
            var reviewerUser = _context.Users.FirstOrDefault(u => u.UserId == reviewer.UserId);
            var reviewedUser = _context.Users.FirstOrDefault(u => u.UserId == reviewed.UserId);

            if (reviewerUser == null || reviewedUser == null)
            {
                return NotFound();
            }
            
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
