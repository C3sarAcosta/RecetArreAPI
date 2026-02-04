using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Title { get; set; } = default!;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(15000)]
        public string Instructions { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int PrepTimeMinutes { get; set; }

        [Range(0, 24 * 60)]
        public int CookTimeMinutes { get; set; }

        [Range(1, 100)]
        public int Servings { get; set; } = 1;

        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        // FK
        [Required]
        public string AuthorId { get; set; } = default!;

        // Navigation
        public ApplicationUser Author { get; set; } = default!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();

    }
}
