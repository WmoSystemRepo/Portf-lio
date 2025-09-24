using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Permicoes
{
    /// <summary>
    /// Interface para serviços relacionados a permissões de acesso no sistema.
    /// </summary>
    public interface IPermicoesService
    {
        #region Métodos de Leitura

        Task<IReadOnlyList<AnyPointStorePermicoes>> ObterTodasPermissaoAsync(CancellationToken cancellationToken);
        Task<AnyPointStorePermicoes?> ObterPermissaoPorIdAsync(int id, CancellationToken cancellationToken);
        Task AtualizarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken);
        Task DeletaPermissaoAsync(int id, CancellationToken cancellationToken);



        Task<IReadOnlyList<PermissaoUsuarioDto>> GetUsersLinkedToPermissionAsync(int permissionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<RoleDto>> GetRolesLinkedToPermissionAsync(int permissionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserDto>> GetUsersToPermissionAsync(int permissionId, CancellationToken cancellationToken);


        #endregion

        #region Métodos de Modificação

        Task CriarPermicoesAsync(PermicoesDto permissao, CancellationToken cancellationToken);






        #endregion

        #region Gerenciamento de Usuários e Roles

        //Task<bool> AssignRoleToPermissionAsync(string roleId, int permissionId, CancellationToken cancellationToken);
        //Task<bool> AssignPermissionToUserAsync(string userId, int permissionId, CancellationToken cancellationToken);
        Task<bool> AssignPermissionToUsersAsync(string[] userIds, int permissionId, CancellationToken cancellationToken);
        Task RemoveUserFromPermissionAsync(int permissionId, string userId, CancellationToken cancellationToken);
        //Task RemoveRoleFromPermissionAsync(string roleId, int permissionId, CancellationToken cancellationToken);

        #endregion
    }
}
