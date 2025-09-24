using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;

namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Menu
{
    public interface IMenuService
    {
        #region Interface Menu
        Task<List<MenuDto>> MenuServicoListasAsync(string userId, CancellationToken cancellationToken);
        Task MenuServicoRegistrarAsync(MenuDto menu, CancellationToken cancellationToken);
        Task<MenuDto> MenuServicoBuscaPorIdAsync(int id, CancellationToken cancellationToken);
        Task MenuServicoDeletarAsync(MenuDto menu, CancellationToken cancellationToken);
        Task MenuServicoEditarAsync(AnyPointStoreMenu menu, CancellationToken cancellationToken);
        Task<ReferenciasMenuDto> MenuServicoObterReferenciasAsync(int id, CancellationToken cancellationToken);
        #endregion

        #region Interface de Referência entre Menu e Regra

        Task MenuRegraServicoRegistrarAsync(List<MenuRegraDto> menus, CancellationToken cancellationToken);
        Task<List<MenuRegraDto>> MenuRegraServicoBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken);
        Task<MenuRegraDto> MenuRegraServicoBuscaPorIdReferenciaAsync(int menuId, string regraId, CancellationToken cancellationToken);
        Task MenuRegraServicoDeletetarAsync(MenuRegraDto menu, CancellationToken cancellationToken);

        #endregion

        #region Interface de Referência entre Menu e Usuário
        Task MenuUsuarioServicoResgistrarAsync(List<MenuUsuarioDto> menus, CancellationToken cancellationToken);
        Task<List<MenuUsuarioDto>> MenuUsuarioServicoBuscarPorIdMenuAsync(int id, CancellationToken cancellationToken);
        Task<AnyPointStoreMenuUsuario> MenuUsuarioServicoBuscaPorIdReferenciaAsync(int menuId, string usuarioId, CancellationToken cancellationToken);
        Task<List<AnyPointStoreMenuUsuario>> MenuUsuarioServicoBuscaPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken);
        Task MenuUsuarioServicoDeletarAsync(AnyPointStoreMenuUsuario menu, CancellationToken cancellationToken);
        #endregion

        #region Interface de Referência entre Menu e Integração

        Task MenuIntegracaoServicoRegistrarAsync(List<MenuIntegracaoDto> menus, CancellationToken cancellationToken);
        Task<AnyPointStoreMenuIntegracao> MenuIntegracaoServicoBuscaPorIdReferenciaAsync(int MenuId, int IntegracaoId, CancellationToken cancellationToken);
        Task<List<MenuIntegracaoDto>> MenuIntegracaoServicoBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken);
        Task MenuIntegracaoServicoDeletarAsync(AnyPointStoreMenuIntegracao menu, CancellationToken cancellationToken);

        #endregion
    }
}