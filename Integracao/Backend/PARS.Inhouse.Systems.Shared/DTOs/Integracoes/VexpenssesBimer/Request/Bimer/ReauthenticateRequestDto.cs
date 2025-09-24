namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer
{
    /// <summary>
    /// DTO utilizado para solicitar uma nova autenticação via token de renovação (refresh token).
    /// Segue o padrão OAuth2.
    /// </summary>
    public class ReauthenticateRequestDto
    {
        /// <summary>
        /// Identificador do cliente (aplicação ou integração) registrado no sistema de autenticação.
        /// </summary>
        public required string client_id { get; set; }

        /// <summary>
        /// Tipo de grant utilizado na autenticação.
        /// Padrão: "refresh_token".
        /// </summary>
        public string grant_type { get; set; } = "refresh_token";

        /// <summary>
        /// Token de renovação obtido na autenticação anterior.
        /// Usado para solicitar um novo access token sem precisar do usuário/senha.
        /// </summary>
        public required string refresh_token { get; set; }
    }
}
