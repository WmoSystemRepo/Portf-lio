using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Permicoes
{
    public interface IPermissaoRepository
    {
        Task RegistrarPermicoesAsync(AnyPointStorePermicoes permicoes, CancellationToken cancellationToken);
        Task AtualizarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken);
        Task DeletarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken);
        Task<AnyPointStorePermicoes?> RecuperarPermissaoPorIdAsync(int id, CancellationToken cancellationToken);


        Task<List<AnyPointStorePermicoes>> GetAllPermissoesAsync(CancellationToken cancellationToken);
        Task<List<PermissaoUsuarioDto>> GetPermissaoUsuarioByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<UserDto>> GetUsuariosForPermissionSelectByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<RoleDto>> GetPermissaoRoleByIdAsync(int id, CancellationToken cancellationToken);


        Task<bool> SetUserPermissionForUserAsync(string userId, int permissionId, CancellationToken cancellationToken);
        Task<bool> SetUserPermissionForUsersAsync(string[] userIds, int permissionId, CancellationToken cancellationToken);
        Task RemoveUserFromPermission(int permissionId, string userId, CancellationToken cancellationToken);
    }
}
