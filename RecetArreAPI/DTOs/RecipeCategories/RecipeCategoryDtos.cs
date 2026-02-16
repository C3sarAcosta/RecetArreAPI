using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.RecipeCategories
{
    public class RecipeCategoryDto
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }
    }

    public class RecipeCategoryCreateDto
    {
        [Required]
        public int RecipeId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
