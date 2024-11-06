using System.ComponentModel.DataAnnotations.Schema;

namespace sports_up_backend.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int LobbyId { get; set; }
        [ForeignKey("LobbyId")]
        public Lobby Lobby { get; set; } = null!;
        public int SenderUserId { get; set; }
        [ForeignKey("SenderUserId")]
        public User Sender { get; set; } = null!;
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
    }
}
