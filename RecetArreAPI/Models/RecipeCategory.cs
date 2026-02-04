namespace RecetArreAPI.Models
{
    public class RecipeCategory
    {
        public int RecipeId { get; set; }
        public int CategoryId { get; set; }

        public Recipe Recipe { get; set; } = default!;
        public Category Category { get; set; } = default!;

    }
}
