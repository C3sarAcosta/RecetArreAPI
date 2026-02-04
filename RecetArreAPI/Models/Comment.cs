using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // FK
        public int RecipeId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        public Recipe Recipe { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

    }
}
