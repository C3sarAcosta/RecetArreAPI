using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.WeeklyRankingEntries
{
    public class WeeklyRankingEntryDto
    {
        public int Id { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public string UserId { get; set; } = default!;
    }

    public class WeeklyRankingEntryCreateDto
    {
        [Required]
        public DateOnly WeekStartDate { get; set; }

        [Range(1, 1000)]
        public int Position { get; set; }

        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
    }

    public class WeeklyRankingEntryUpdateDto
    {
        [Required]
        public DateOnly WeekStartDate { get; set; }

        [Range(1, 1000)]
        public int Position { get; set; }

        [Range(0, int.MaxValue)]
        public int Points { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
    }
}
