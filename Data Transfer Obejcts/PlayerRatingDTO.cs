namespace sports_up_backend.Data_Transfer_Obejcts
{
    public class PlayerRatingDTO
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
