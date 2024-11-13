using sports_up_backend.Models;

public class LobbyPlayerWithUserDTO
{
    public int LobbyId { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public LobbyPlayerStatus Status { get; set; }
}
