using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class UserProfile
    {
        [Key]
        public string UserId { get; set; } = default!;

        [StringLength(500)]
        public string? Bio { get; set; }

        [StringLength(300)]
        [Url]
        public string? AvatarUrl { get; set; }

        [StringLength(120)]
        public string? Location { get; set; }

        public DateOnly? BirthDate { get; set; }

        [StringLength(200)]
        [Url]
        public string? WebsiteUrl { get; set; }

        [StringLength(200)]
        public string? DietaryPreferences { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation
        public ApplicationUser User { get; set; } = default!;

    }
}
