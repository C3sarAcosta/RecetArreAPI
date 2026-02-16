using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.RecipeIngredients
{
    public class RecipeIngredientDto
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = default!;
        public string? Preparation { get; set; }
    }

    public class RecipeIngredientCreateDto
    {
        [Required]
        public int RecipeId { get; set; }

        [Required]
        public int IngredientId { get; set; }

        [Range(0.01, 9999999)]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = default!;

        [StringLength(200)]
        public string? Preparation { get; set; }
    }

    public class RecipeIngredientUpdateDto
    {
        [Range(0.01, 9999999)]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = default!;

        [StringLength(200)]
        public string? Preparation { get; set; }
    }
}
