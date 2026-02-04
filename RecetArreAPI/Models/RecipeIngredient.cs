using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI.Models
{
    public class RecipeIngredient
    {

        public int RecipeId { get; set; }
        public int IngredientId { get; set; }

        [Column(TypeName = "numeric(10,2)")]
        [Range(0.01, 9999999)]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(20)]
        public string Unit { get; set; } = "unit";

        [StringLength(200)]
        public string? Preparation { get; set; }

        public Recipe Recipe { get; set; } = default!;
        public Ingredient Ingredient { get; set; } = default!;

    }
}
