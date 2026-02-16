using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Ingredients
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Notes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class IngredientCreateDto
    {
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Notes { get; set; }
    }

    public class IngredientUpdateDto
    {
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Notes { get; set; }
    }
}
