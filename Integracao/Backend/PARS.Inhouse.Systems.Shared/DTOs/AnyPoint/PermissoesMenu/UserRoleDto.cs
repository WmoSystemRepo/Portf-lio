namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu
{
    /// <summary>
    /// DTO que representa a associação entre um usuário e uma role (perfil).
    /// Utilizado para controle de acesso baseado em perfis no sistema.
    /// </summary>
    public class UserRoleDto
    {
        /// <summary>
        /// Identificador do usuário que será vinculado a uma role.
        /// Geralmente corresponde ao ID do Identity (ex: GUID).
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Nome da role (perfil) atribuída ao usuário.
        /// Exemplo: "Admin", "Gestor", "Operador".
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// Identificador único da role (pode ser usado ao invés do nome, conforme a estratégia do Identity).
        /// </summary>
        public string? RoleId { get; set; }
    }
}
