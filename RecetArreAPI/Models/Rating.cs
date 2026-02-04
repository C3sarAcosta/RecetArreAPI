using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Rating
    {

        public int Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // FK
        public int RecipeId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        public Recipe Recipe { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

    }
}
