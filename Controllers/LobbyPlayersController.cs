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
    public class LobbyPlayersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LobbyPlayersController(ApplicationDbContext context)
        {
            _context = context;
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

        // POST: api/LobbyPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LobbyPlayer>> PostLobbyPlayer(LobbyPlayer lobbyPlayer)
        {
            _context.LobbyPlayers.Add(lobbyPlayer);
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

        private bool LobbyPlayerExists(int id)
        {
            return _context.LobbyPlayers.Any(e => e.LobbyId == id);
        }
    }
}
