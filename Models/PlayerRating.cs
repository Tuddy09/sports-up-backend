using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sports_up_backend.Models
{
    public class PlayerRating
    {
        [Key]
        public int RatingId { get; set; }
        public int RatedByUserId { get; set; }
        [ForeignKey("RatedByUserId")]
        [JsonIgnore]
        public User RatedByUser { get; set; } = null!;
        public int RatedUserId { get; set; }
        [ForeignKey("RatedUserId")]
        [JsonIgnore]
        public User RatedUser { get; set; } = null!;
            
        public string Category { get; set; }
        public int Stars { get; set; }
    }
}
