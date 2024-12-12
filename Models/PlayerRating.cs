using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sports_up_backend.Models
{
    public class PlayerRating
    {

        public PlayerRating() { }

        public PlayerRating(int ratedByUserId, User ratedByUser, int ratedUserId, User ratedUser, string message, int stars)
        {
            RatedByUserId = ratedByUserId;
            RatedUserId = ratedUserId;
            RatedByUser = ratedByUser;
            RatedUser = ratedUser;
            Message = message;
            Stars = stars;
        }

        [Key]
        public int RatingId { get; set; }

        public int RatedByUserId { get; set; }
        [ForeignKey("RatedByUserId")]
        [JsonIgnore]  // Ignore during serialization
        public User RatedByUser { get; set;  } = null!;

        public int RatedUserId { get; set; }
        [ForeignKey("RatedUserId")]
        [JsonIgnore]  // Ignore during serialization
        public User RatedUser { get; set;  } = null!;
        public string Message { get; set; }
        public int Stars { get; set; }
    }
}