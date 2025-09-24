using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Usuario
{
    public interface IUsuarioService
    {
        #region Interface de Referência entre Usuário e Integração

        Task UsuarioIntegracaoServicoRegistrarAsync(List<UsuarioIntegracaoDto> menus, CancellationToken cancellationToken);
        Task<AnyPointStoreUsuarioIntegracao> UsuarioIntegracaoServicoBuscaPorIdReferenciaAsync(string usuarioId, int IntegracaoId, CancellationToken cancellationToken);
        Task<List<AnyPointStoreUsuarioIntegracao>> UsuarioIntegracaoServicoBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken);
        Task UsuarioIntegracaoServicoExcluirAsync(AnyPointStoreUsuarioIntegracao menu, CancellationToken cancellationToken);

        #endregion

        #region Interface de Referência entre Usuário e Permissao
        Task UsuarioPermissaoServicoRegistrarAsync(List<UsuarioPermissaoDto> integracoes, CancellationToken cancellationToken);
        Task<UsuarioPermissaoDto> UsuarioPermissaoServicoBuscaPorIdReferenciaAsync(string usuarioId, int idReferencia, CancellationToken cancellationToken);
        Task<List<UsuarioPermissaoDto?>> UsuarioPermissaoServicoBuscarPorIdMenuAsync(string idMenu, CancellationToken cancellationToken);
        Task UsuarioPermissaoServicoExcluirAsync(UsuarioPermissaoDto usuarioPermissao, CancellationToken cancellationToken);

        #endregion

        #region Interface de Referência entre Usuário e Regra
        Task UsuarioRegraServicoRegistrarAsync(List<UsuarioRegraDto> integracoes, CancellationToken cancellationToken);
        Task<UsuarioRegraDto> UsuarioRegraServicoBuscaPorIdReferenciaAsync(string usuarioId, string regraId, CancellationToken cancellationToken);
        Task<List<UsuarioRegraDto?>> UsuarioRegraServicoBuscarPorIdMenuAsync(string idMenu, CancellationToken cancellationToken);
        Task UsuarioRegraServicoExcluirAsync(UsuarioRegraDto usuarioRegra, CancellationToken cancellationToken);

        #endregion
    }
}
