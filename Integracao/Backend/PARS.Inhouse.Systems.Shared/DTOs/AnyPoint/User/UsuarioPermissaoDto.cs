namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO que representa a associação entre um usuário e uma permissão individual no sistema.
    /// Utilizado para controlar permissões diretas concedidas a usuários, além das herdadas por roles.
    /// </summary>
    public class UsuarioPermissaoDto
    {
        /// <summary>
        /// Identificador único da associação entre o usuário e a permissão.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do usuário que está recebendo a permissão.
        /// Normalmente corresponde ao ID do Identity.
        /// </summary>
        public string UsuarioId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador da permissão atribuída ao usuário.
        /// Deve corresponder a um item previamente cadastrado em Permissões.
        /// </summary>
        public int PermissaoId { get; set; }

        /// <summary>
        /// Data de criação do vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última alteração no vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Indica se o vínculo entre usuário e permissão está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
