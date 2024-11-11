namespace sports_up_backend.Data_Transfer_Obejcts
{
    public class LobbyDTO
    {
        public int OwnerId { get; set; }
        public string Sport { get; set; }
        public DateOnly Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int TotalSpots { get; set; }
        public int AvailableSpots { get; set; }
        public string SkillLevel { get; set; }
    }
}
