using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Notes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    }
}
