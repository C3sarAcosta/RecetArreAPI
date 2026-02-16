namespace RecetArreAPI.DTOs.Identity
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; } = default!;
        public DateTime Expiracion { get; set; }
        public string UsuarioId { get; set; } = default!;
    }
}
