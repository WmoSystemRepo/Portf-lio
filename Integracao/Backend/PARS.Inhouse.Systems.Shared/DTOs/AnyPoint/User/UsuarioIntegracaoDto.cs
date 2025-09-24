namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO que representa a associação entre um usuário e uma integração no sistema AnyPoint.
    /// Utilizado para controlar quais usuários têm acesso a determinadas integrações.
    /// </summary>
    public class UsuarioIntegracaoDto
    {
        /// <summary>
        /// Identificador único da associação (opcional em criação).
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identificador do usuário associado à integração.
        /// Normalmente corresponde ao ID do Identity.
        /// </summary>
        public string UsuarioId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador da integração atribuída ao usuário.
        /// </summary>
        public int IntegracaoId { get; set; }

        /// <summary>
        /// Data de criação da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Indica se a associação está ativa.
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
