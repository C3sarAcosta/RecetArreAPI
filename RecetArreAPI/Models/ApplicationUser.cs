using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(60)]
        public string? DisplayName { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation
        public UserProfile? Profile { get; set; }
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();
        public ICollection<WeeklyRankingEntry> WeeklyRankingEntries { get; set; } = new List<WeeklyRankingEntry>();

    }
}
