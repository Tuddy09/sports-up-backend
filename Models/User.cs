namespace sports_up_backend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int AvatarId { get; set; }
        public ICollection<Lobby> OwnedLobbies { get; } = new List<Lobby>();
        
        public ICollection<LobbyPlayer> LobbyPlayers { get; } = new List<LobbyPlayer>();
        public ICollection<PlayerRating> RatingsGiven { get; } = new List<PlayerRating>();
        public ICollection<PlayerRating> RatingsReceived { get; } = new List<PlayerRating>();
        public ICollection<Message> SentMessages { get; } = new List<Message>();
    }
}
