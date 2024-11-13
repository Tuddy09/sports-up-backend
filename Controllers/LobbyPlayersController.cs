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
    public class LobbyPlayersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LobbyPlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // PUT: api/LobbyPlayers/Accept
        [HttpPut("Accept")]
        public async Task<IActionResult> AcceptLobbyPlayer(LobbyPlayerDTO lobbyPlayerDTO)
        {
            var lobbyPlayer = await _context.LobbyPlayers
                .Where(lp => lp.LobbyId == lobbyPlayerDTO.LobbyId && lp.UserId == lobbyPlayerDTO.UserId)
                .FirstOrDefaultAsync();
            if (lobbyPlayer == null)
            {
                return NotFound();
            }
            lobbyPlayer.Status = LobbyPlayerStatus.Accepted;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/LobbyPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LobbyPlayer>>> GetLobbyPlayers()
        {
            return await _context.LobbyPlayers.ToListAsync();
        }

        // GET: api/LobbyPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LobbyPlayer>> GetLobbyPlayer(int id)
        {
            var lobbyPlayer = await _context.LobbyPlayers.FindAsync(id);

            if (lobbyPlayer == null)
            {
                return NotFound();
            }

            return lobbyPlayer;
        }

        // PUT: api/LobbyPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobbyPlayer(int id, LobbyPlayer lobbyPlayer)
        {
            if (id != lobbyPlayer.LobbyId)
            {
                return BadRequest();
            }

            _context.Entry(lobbyPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LobbyPlayerExists(id))
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

        // POST: api/LobbyPlayers/Request
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Request")]
        public async Task<ActionResult<LobbyPlayer>> PostLobbyPlayer(LobbyPlayerDTO lobbyPlayerDTO)
        {
            // verify that the owner of the lobby is not trying to join the lobby and that the user has not already requested to join the lobby
            var lobby = await _context.Lobbies.FindAsync(lobbyPlayerDTO.LobbyId);
            if (lobby.OwnerId == lobbyPlayerDTO.UserId)
            {
                return BadRequest("Owner of the lobby cannot join the lobby");
            }
            if (_context.LobbyPlayers.Any(lp => lp.LobbyId == lobbyPlayerDTO.LobbyId && lp.UserId == lobbyPlayerDTO.UserId))
            {
                return BadRequest("User has requested already to join the lobby/is accepted already");
            }
            if (lobby.AvailableSpots <= 0)
            {
                return BadRequest("No available spots in the lobby");
            }
            var lobbyPlayer = new LobbyPlayer
            {
                LobbyId = lobbyPlayerDTO.LobbyId,
                UserId = lobbyPlayerDTO.UserId,
                Status = LobbyPlayerStatus.Pending
            };
            _context.LobbyPlayers.Add(lobbyPlayer);
            lobby.AvailableSpots--;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LobbyPlayerExists(lobbyPlayer.LobbyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLobbyPlayer", new { id = lobbyPlayer.LobbyId }, lobbyPlayer);
        }

        // DELETE: api/LobbyPlayers/Reject
        [HttpDelete("Reject")]
        public async Task<IActionResult> RejectLobbyPlayer(LobbyPlayerDTO lobbyPlayerDTO)
        {
            var lobbyPlayer = await _context.LobbyPlayers
                .Where(lp => lp.LobbyId == lobbyPlayerDTO.LobbyId && lp.UserId == lobbyPlayerDTO.UserId)
                .FirstOrDefaultAsync();
            if (lobbyPlayer == null)
            {
                return NotFound();
            }
            _context.LobbyPlayers.Remove(lobbyPlayer);
            // increase available spots in the lobby
            var lobby = await _context.Lobbies.FindAsync(lobbyPlayerDTO.LobbyId);
            lobby.AvailableSpots++;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/LobbyPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLobbyPlayer(int id)
        {
            var lobbyPlayer = await _context.LobbyPlayers.FindAsync(id);
            if (lobbyPlayer == null)
            {
                return NotFound();
            }

            _context.LobbyPlayers.Remove(lobbyPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // GET: api/LobbyPlayers/Requests/{lobbyId}
        // This endpoint retrieves all pending join requests for a specific lobby.
        [HttpGet("Requests/{lobbyId}")]
        public async Task<ActionResult<IEnumerable<LobbyPlayerWithUserDTO>>> GetLobbyJoinRequests(int lobbyId)
        {
            var joinRequests = await _context.LobbyPlayers
                .Where(lp => lp.LobbyId == lobbyId && lp.Status == LobbyPlayerStatus.Pending)
                .Include(lp => lp.User)
                .Select(lp => new LobbyPlayerWithUserDTO
                {
                    LobbyId = lp.LobbyId,
                    UserId = lp.UserId,
                    Username = lp.User.Username,
                    Email = lp.User.Email,
                    Status = lp.Status
                })
                .ToListAsync();

            if (!joinRequests.Any())
            {
                return NotFound("No pending join requests found for this lobby.");
            }

            return Ok(joinRequests);
        }


        private bool LobbyPlayerExists(int id)
        {
            return _context.LobbyPlayers.Any(e => e.LobbyId == id);
        }
    }
}
