using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class WeeklyRankingEntry
    {
        public int Id { get; set; }

        [Required]
        public DateOnly WeekStartDate { get; set; }

        [Range(1, 1000)]
        public int Position { get; set; }

        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;

    }
}
