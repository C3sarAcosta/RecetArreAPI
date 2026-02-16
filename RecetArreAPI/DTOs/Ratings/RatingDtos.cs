using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Ratings
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; } = default!;
    }

    public class RatingCreateDto
    {
        [Range(1, 5)]
        public int Stars { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
    }

    public class RatingUpdateDto
    {
        [Range(1, 5)]
        public int Stars { get; set; }
    }
}
