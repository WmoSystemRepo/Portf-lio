namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu
{
    /// <summary>
    /// DTO utilizado para atribuição de permissões a usuários individualmente ou em lote.
    /// Permite associar permissões diretamente a usuários e/ou perfis (roles).
    /// </summary>
    public class PermissoesUsuarioDto
    {
        /// <summary>
        /// Lista de identificadores de usuários que receberão a permissão.
        /// Usado para atribuições em massa.
        /// </summary>
        public string[]? UserIds { get; set; }

        /// <summary>
        /// Identificador da permissão que será atribuída.
        /// Deve referenciar uma permissão previamente cadastrada.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Identificador de um único usuário a ser associado à permissão.
        /// Útil em operações unitárias.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Identificador da role (perfil) relacionado à permissão, caso a permissão deva ser herdada por perfil.
        /// </summary>
        public string? RoleId { get; set; }
    }
}
