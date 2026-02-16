using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.ApplicationUsers
{
    public class ApplicationUserDto
    {
        public string Id { get; set; } = default!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string DisplayName { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; }
    }

    public class ApplicationUserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        public string? UserName { get; set; }

        [Required]
        [StringLength(60)]
        public string DisplayName { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }

    public class ApplicationUserUpdateDto
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(60)]
        public string DisplayName { get; set; } = default!;
    }
}
