namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu
{
    /// <summary>
    /// DTO que representa um perfil (role) do sistema.
    /// As roles são utilizadas para agrupar permissões e controlar o acesso de usuários a funcionalidades.
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Identificador único da role (pode ser um GUID ou nome, dependendo da implementação do Identity).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nome da role (perfil), como "Admin", "Gestor", "Operador", etc.
        /// </summary>
        public string? Name { get; set; }
    }
}
