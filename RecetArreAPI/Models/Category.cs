using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Description { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<RecipeCategory> RecipeCategories { get; set; } = new List<RecipeCategory>();

    }
}
