using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.Identity
{
    public class UserCredentialsDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
