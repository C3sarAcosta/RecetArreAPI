using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Medals
{
    public class MedalDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class MedalCreateDto
    {
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
    }

    public class MedalUpdateDto
    {
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
    }
}
