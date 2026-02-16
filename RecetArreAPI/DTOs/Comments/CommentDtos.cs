using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; } = default!;
    }

    public class CommentCreateDto
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = default!;

        [Required]
        public int RecipeId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
    }

    public class CommentUpdateDto
    {
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = default!;
    }
}
