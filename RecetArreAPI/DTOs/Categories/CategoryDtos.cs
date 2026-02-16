using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class CategoryCreateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Description { get; set; }
    }

    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; } = default!;

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
