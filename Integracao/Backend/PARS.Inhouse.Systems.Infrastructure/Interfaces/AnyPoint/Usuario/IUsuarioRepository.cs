using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Usuario
{
    public interface IUsuarioRepository
    {
        #region Interfaces de Repositório de Menu com Referência à Integracão

        Task UsuarioIntegracaoRepositorioRegistrarAsync(List<UsuarioIntegracaoDto> usuario, CancellationToken cancellationToken);
        Task<List<AnyPointStoreUsuarioIntegracao>?> UsuarioIntegracaoRepositorioBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken);
        Task<AnyPointStoreUsuarioIntegracao> UsuarioIntegracaoRepositorioBuscarPorIdReferenciaAsync(string usuarioId, int integracaoId, CancellationToken cancellationToken);
        Task UsuarioIntegracaoRepositorioExcluirAsync(AnyPointStoreUsuarioIntegracao usuarioIntegracao, CancellationToken cancellationToken);

        #endregion

        Task UsuarioPermissaoRepositorioRegistrarAsync(List<UsuarioPermissaoDto> permissao, CancellationToken cancellationToken);
        Task<UsuarioPermissaoDto> UsuarioPermissaoRepositorioBuscarPorIdReferenciaAsync(string usuarioId, int permissaoId, CancellationToken cancellationToken);
        Task<List<UsuarioPermissaoDto>> UsuarioPermissaoRepositorioBuscarPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken);
        Task UsuarioPermissaoRepositorioExcluirAsync(UsuarioPermissaoDto usuarioPermissao, CancellationToken cancellationToken);


        Task UsuarioRegraRepositorioRegistrarAsync(List<UsuarioRegraDto> regra, CancellationToken cancellationToken);
        Task<UsuarioRegraDto> UsuarioRegraRepositorioBuscarPorIdReferenciaAsync(string usuarioId, string regraId, CancellationToken cancellationToken);
        Task<List<UsuarioRegraDto>> UsuarioRegraRepositorioBuscarPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken);
        Task UsuarioRegraRepositorioExcluirAsync(UsuarioRegraDto usuarioPermissao, CancellationToken cancellationToken);
    }
}