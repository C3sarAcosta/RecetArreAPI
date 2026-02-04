namespace RecetArreAPI.Models
{
    public class UserMedal
    {
        public string UserId { get; set; } = default!;
        public int MedalId { get; set; }

        public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

        public string? AssignedByUserId { get; set; }

        public ApplicationUser User { get; set; } = default!;
        public Medal Medal { get; set; } = default!;
        public ApplicationUser? AssignedByUser { get; set; }

    }
}
