namespace RecetArreAPI.DTOs.Identity
{
    public class AuthenticationResponseDto
    {
        public string Token { get; set; } = default!;
        public DateTime Expiration { get; set; }
        public string UserId { get; set; } = default!;
    }
}
