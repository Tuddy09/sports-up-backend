using System.ComponentModel.DataAnnotations.Schema;

namespace sports_up_backend.Models
{
    public class LobbyPlayer
    {
        public int LobbyPlayerId { get; set; }
        public int LobbyId { get; set; }
        [ForeignKey("LobbyId")]
        public Lobby Lobby { get; set; } = null!;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        public LobbyPlayerStatus Status { get; set; }
    }

    public enum LobbyPlayerStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}


