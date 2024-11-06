using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sports_up_backend.Models
{
    public class PlayerRating
    {
        [Key]
        public int RatingId { get; set; }
        public int RatedByUserId { get; set; }
        [ForeignKey("RatedByUserId")]
        public User RatedByUser { get; set; } = null!;
        public int RatedUserId { get; set; }
        [ForeignKey("RatedUserId")]
        public User RatedUser { get; set; } = null!;
        public string Category { get; set; }
        public int Stars { get; set; }
    }
}
