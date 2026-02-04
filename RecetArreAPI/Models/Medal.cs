using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.Models
{
    public class Medal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Code { get; set; } = default!;

        [Required]
        [StringLength(80)]
        public string Name { get; set; } = default!;

        [StringLength(300)]
        public string? Description { get; set; }

        [StringLength(300)]
        [Url]
        public string? IconUrl { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<UserMedal> UserMedals { get; set; } = new List<UserMedal>();

    }
}
