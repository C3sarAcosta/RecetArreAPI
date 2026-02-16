using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Recipes
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string Instructions { get; set; } = default!;
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public string AuthorId { get; set; } = default!;
    }

    public class RecipeCreateDto
    {
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

        [Required]
        public string AuthorId { get; set; } = default!;
    }

    public class RecipeUpdateDto
    {
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

        [Required]
        public string AuthorId { get; set; } = default!;
    }
}
