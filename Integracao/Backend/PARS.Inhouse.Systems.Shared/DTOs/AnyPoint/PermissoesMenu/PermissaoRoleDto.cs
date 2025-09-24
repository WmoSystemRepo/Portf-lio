namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu
{
    /// <summary>
    /// DTO que representa a associação entre uma role (perfil) e uma permissão específica no sistema.
    /// Utilizado para configurar os acessos que cada perfil de usuário pode ter.
    /// </summary>
    public class PermissaoRoleDto
    {
        /// <summary>
        /// Identificador da role (perfil de usuário) à qual a permissão será atribuída.
        /// Pode ser o nome ou o ID da role conforme a implementação do Identity.
        /// </summary>
        public string? RoleId { get; set; }

        /// <summary>
        /// Identificador da permissão que será atribuída à role.
        /// </summary>
        public int PermissionId { get; set; }
    }
}
