namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO que representa a associação entre um usuário e uma regra (regra de negócio ou perfil) no sistema.
    /// </summary>
    public class UsuarioRegraDto
    {
        /// <summary>
        /// Identificador único da associação entre usuário e regra.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador do usuário que será vinculado à regra.
        /// Normalmente corresponde ao ID do Identity.
        /// </summary>
        public string UsuarioId { get; set; } = string.Empty;

        /// <summary>
        /// Identificador da regra (regra de negócio, agrupamento de permissões, ou perfil específico).
        /// </summary>
        public string RegraId { get; set; } = string.Empty;

        /// <summary>
        /// Data de criação do vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última modificação do vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Indica se o vínculo entre o usuário e a regra está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
