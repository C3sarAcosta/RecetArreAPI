using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.UserProfiles
{
    public class UserProfileDto
    {
        public string UserId { get; set; } = default!;
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Location { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? DietaryPreferences { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }

    public class UserProfileCreateDto
    {
        [Required]
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
    }

    public class UserProfileUpdateDto
    {
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
    }
}
