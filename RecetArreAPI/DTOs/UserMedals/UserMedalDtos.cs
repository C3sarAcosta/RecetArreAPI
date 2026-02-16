using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI.DTOs.UserMedals
{
    public class UserMedalDto
    {
        public string UserId { get; set; } = default!;
        public int MedalId { get; set; }
        public DateTime AssignedAtUtc { get; set; }
        public string? AssignedByUserId { get; set; }
    }

    public class UserMedalCreateDto
    {
        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public int MedalId { get; set; }

        public string? AssignedByUserId { get; set; }
    }

    public class UserMedalUpdateDto
    {
        public DateTime AssignedAtUtc { get; set; }
        public string? AssignedByUserId { get; set; }
    }
}
