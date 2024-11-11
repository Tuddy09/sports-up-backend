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
    public class LobbiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LobbiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lobbies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetLobbies()
        {
            return await _context.Lobbies.ToListAsync();
        }

        // GET: api/Lobbies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lobby>> GetLobby(int id)
        {
            var lobby = await _context.Lobbies.FindAsync(id);

            if (lobby == null)
            {
                return NotFound();
            }

            return lobby;
        }

        // GET: api/Lobbies/Owned
        [HttpGet("Owned/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetOwnedLobbies(int ownerId)
        {
            return await _context.Lobbies.Where(l => l.OwnerId == ownerId).ToListAsync();
        }

        // GET: api/Lobbies/Joined
        [HttpGet("Joined/{userId}")]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetJoinedLobbies(int userId)
        {
            // Get all the lobbies that the user has joined and status is accepted in LobbyPlayer
            return await _context.Lobbies
                .Where(l => l.LobbyPlayers.Any(lp => lp.UserId == userId && lp.Status == LobbyPlayerStatus.Accepted))
                .ToListAsync();
        }

        // PUT: api/Lobbies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLobby(int id, Lobby lobby)
        {
            if (id != lobby.LobbyId)
            {
                return BadRequest();
            }

            _context.Entry(lobby).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LobbyExists(id))
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

        // POST: api/Lobbies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lobby>> PostLobby(LobbyDTO lobbyDTO)
        {
            var lobby = new Lobby
            {
                OwnerId = lobbyDTO.OwnerId,
                Sport = lobbyDTO.Sport,
                Date = lobbyDTO.Date,
                Time = lobbyDTO.Time,
                Location = lobbyDTO.Location,
                Latitude = lobbyDTO.Latitude,
                Longitude = lobbyDTO.Longitude,
                TotalSpots = lobbyDTO.TotalSpots,
                AvailableSpots = lobbyDTO.AvailableSpots,
                SkillLevel = lobbyDTO.SkillLevel,
                CreatedAt = DateTime.UtcNow
            };

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLobby", new { id = lobby.LobbyId }, lobby);
        }

        // DELETE: api/Lobbies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLobby(int id)
        {
            var lobby = await _context.Lobbies.FindAsync(id);
            if (lobby == null)
            {
                return NotFound();
            }

            _context.Lobbies.Remove(lobby);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LobbyExists(int id)
        {
            return _context.Lobbies.Any(e => e.LobbyId == id);
        }
    }
}
