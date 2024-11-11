using System.ComponentModel.DataAnnotations.Schema;

namespace sports_up_backend.Models
{
    public class Lobby
    {
        public int LobbyId { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;
        public string Sport { get; set; }
        public DateOnly Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal Longitude { get; set; }
        public int TotalSpots { get; set; }
        public int AvailableSpots { get; set; }
        public string SkillLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<LobbyPlayer> LobbyPlayers { get; } = new List<LobbyPlayer>();
        public ICollection<Message> Messages { get; } = new List<Message>();
    }
}
