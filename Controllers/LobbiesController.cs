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
using sports_up_backend.Constants;

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

        // GET: api/Lobbies/{userId}/Available
        [HttpGet("{userId}/Available")]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetAvailableLobbies(int userId)
        {
            // return all the lobbies that are not full
            // and the user is not the owner
            // and the lobby is not in the past
            List<Lobby> lobbies = await _context.Lobbies
                .Where(l => l.AvailableSpots > 0)
                .Where(l => l.OwnerId != userId)
                .Where(l => l.Date >= DateOnly.FromDateTime(DateTime.Now))
                .ToListAsync();
            // return all the lobbies that the user has not joined
            List<Lobby> availableLobbies = new List<Lobby>();
            foreach (var lobby in lobbies)
            {
                var lobbyPlayer = await _context.LobbyPlayers
                    .Where(lp => lp.LobbyId == lobby.LobbyId && lp.UserId == userId)
                    .FirstOrDefaultAsync();
                if (lobbyPlayer == null)
                {
                    availableLobbies.Add(lobby);
                }
            }
            return availableLobbies;
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
        
        // GET: api/Lobbies/Finished
        [HttpGet("Finished/{userId}")]
        public async Task<ActionResult<IEnumerable<Lobby>>> GetUserFinishedLobbies(int userId)
        {
            var finishedLobbies = await _context.Lobbies
                .Where(l => 
                l.LobbyPlayers.Any(lp => lp.UserId == userId) && 
                l.Status == LobbyStatus.Finished)
                .ToListAsync();

            return finishedLobbies;
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
        
        //Put: api/Lobbies/changeStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{lobbyId}/{userId}")]
        public async Task<IActionResult> MarkLobbyAsFinished(int lobbyId, int userId)
        {
            var lobby = await _context.Lobbies.FindAsync(lobbyId);
            var user = await _context.Users.FindAsync(userId);
            if (lobby == null || user == null)
            {
                return NotFound();    
            }
            if (lobby.OwnerId != userId)
            {
                return Unauthorized();
            }
            
            lobby.Status = LobbyStatus.Finished;
            _context.Update(lobby);
            await _context.SaveChangesAsync();
            
            return NoContent();
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
            var user = await _context.Users.FindAsync(lobbyDTO.OwnerId);
            if (user == null)
            {
                return NotFound();
            }
            
            var lobby = new Lobby
            {
                OwnerId = lobbyDTO.OwnerId,
                Sport = lobbyDTO.Sport,
                Date = lobbyDTO.Date,
                Time = lobbyDTO.Time,
                Location = lobbyDTO.Location,
                Latitude = PredefinedLocationCoordinates.Coordinates[lobbyDTO.Location].Latitude,
                Longitude = PredefinedLocationCoordinates.Coordinates[lobbyDTO.Location].Longitude,
                TotalSpots = lobbyDTO.TotalSpots,
                AvailableSpots = lobbyDTO.AvailableSpots -1 ,
                SkillLevel = lobbyDTO.SkillLevel,
                CreatedAt = DateTime.UtcNow,
            };

            var newLobbyPlayer = new LobbyPlayer
            {
                Lobby = lobby,
                UserId = lobby.OwnerId,
                Status = LobbyPlayerStatus.Accepted
            };
            
            //Nu sunt sigur daca aceste 2 adduri sunt necesare, eu deja adaug relatia dintre lobby ul creat si owner mai
            //jos
            user.OwnedLobbies.Add(lobby);
            lobby.LobbyPlayers.Add(newLobbyPlayer);
            
            _context.LobbyPlayers.Add(newLobbyPlayer);
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
            
            var lobbyPlayers = _context.LobbyPlayers.Where(lp => lp.LobbyId == id);
            _context.LobbyPlayers.RemoveRange(lobbyPlayers);
            
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
