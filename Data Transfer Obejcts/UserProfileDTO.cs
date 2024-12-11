using Azure.Identity;

namespace sports_up_backend.Data_Transfer_Obejcts
{
    public class UserProfileDTO
    {
        public string Username { get; set; }
        public int Age { get; set; }
        public int AvatarId { get; set; }
        public int TotalMatchesPlayed { get; set; }
        public string PreferredSport { get; set; }
        public int Rating { get; set; }
    }
}
