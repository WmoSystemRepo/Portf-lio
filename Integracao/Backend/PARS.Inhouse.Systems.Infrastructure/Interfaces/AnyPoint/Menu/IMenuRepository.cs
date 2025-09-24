using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Menu
{
    public interface IMenuRepository
    {

        #region Interfaces de Repositório de Menu

        Task<List<MenuDto>> RepositorioListaMenusAsync(string userId, CancellationToken cancellationToken);
        Task RepositorioRegistrarMenuAsync(MenuDto menu, CancellationToken cancellationToken);
        Task<MenuDto?> RepositorioObterMenuPorIdAsync(int id, CancellationToken cancellationToken);
        Task RepositorioAtualizarMenuAsync(AnyPointStoreMenu menu, CancellationToken cancellationToken);
        Task RepositorioDeletarMenuAsync(MenuDto menu, CancellationToken cancellationToken);
        Task<List<MenuDto>> RepositorioObterSubMenusPorIdAsync(int idMenu, CancellationToken cancellationToken);

        #endregion

        #region Interfaces de Repositório de Menu com Referência à Regra

        Task<List<MenuRegraDto>> MenuRepositorioListasAsync(string userId, CancellationToken cancellationToken);
        Task MenuRegistrarAsync(List<MenuRegraDto> menu, CancellationToken cancellationToken);
        Task<List<MenuRegraDto>> MenuRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken);
        Task<MenuRegraDto> MenuRegraRepositorioBuscarPorIdReferenciaAsync(int menuId, string regraId, CancellationToken cancellationToken);
        Task<List<MenuRegraDto>> MenuRepositorioEditarAsync(AnyPointStoreMenuRegra menuBD, CancellationToken cancellationToken);
        Task MenuRegraRepositorioDeletarAsync(MenuRegraDto menu, CancellationToken cancellationToken);

        #endregion

        #region Interfaces de Repositório de Menu com Referência à Usuário 

        Task MenuUsuarioRepositorioRegistrarAsync(List<MenuUsuarioDto> menus, CancellationToken cancellationToken);
        Task<List<MenuUsuarioDto>> MenuUsuarioRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken);
        Task<AnyPointStoreMenuUsuario> MenuUsuarioRepositorioBuscarPorIdReferenciaAsync(int menuId, string idReferencia, CancellationToken cancellationToken);
        Task<List<AnyPointStoreMenuUsuario>> MenuUsuarioRepositorioBuscarPorIdUsuariosync(string idUsuario, CancellationToken cancellationToken);
        Task MenuUsuarioRepositorioDeletarAsync(AnyPointStoreMenuUsuario menuIntegracao, CancellationToken cancellationToken);

        #endregion

        #region Interfaces de Repositório de Menu com Referência à Integracão

        Task MenuIntegracaoRepositorioRegistrarAsync(List<MenuIntegracaoDto> menus, CancellationToken cancellationToken);
        Task<List<MenuIntegracaoDto>?> MenuIntegracaoRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken);
        Task<AnyPointStoreMenuIntegracao> MenuIntegracaoRepositorioBuscarPorIdReferenciaAsync(int MenuId, int IntegracaoId, CancellationToken cancellationToken);
        Task MenuIntegracaoRepositorioDeletarAsync(AnyPointStoreMenuIntegracao menuIntegracao, CancellationToken cancellationToken);

        #endregion

    }
}