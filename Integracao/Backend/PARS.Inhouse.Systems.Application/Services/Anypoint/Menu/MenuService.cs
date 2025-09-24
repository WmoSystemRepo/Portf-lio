using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repositorio;

        public MenuService(IMenuRepository repositorio)
        {
            _repositorio = repositorio;
        }

        #region Métodos do Menu

        public async Task<List<MenuDto>> MenuServicoListasAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _repositorio.RepositorioListaMenusAsync(userId, cancellationToken);
        }

        public async Task MenuServicoRegistrarAsync(MenuDto menu, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.RepositorioRegistrarMenuAsync(menu, cancellationToken);
        }

        public async Task<MenuDto> MenuServicoBuscaPorIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _repositorio.RepositorioObterMenuPorIdAsync(id, cancellationToken);
        }

        public async Task MenuServicoDeletarAsync(MenuDto menu, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.RepositorioDeletarMenuAsync(menu, cancellationToken);
        }

        public async Task MenuServicoEditarAsync(AnyPointStoreMenu menu, CancellationToken cancellationToken)
        {
            //ArgumentNullException.ThrowIfNull(menu, nameof(menu));

            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.RepositorioAtualizarMenuAsync(menu, cancellationToken);
        }

        public async Task<ReferenciasMenuDto> MenuServicoObterReferenciasAsync(int id, CancellationToken cancellationToken)
        {
            return await PreencherReferenciasMenuAsync(id, cancellationToken);
        }


        #endregion

        #region Métodos com Referência entre Menu e Regra

        public async Task<List<MenuRegraDto>> MenuRegraServicoBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _repositorio.MenuRepositorioBuscarPorIdMenuAsync(idMenu, cancellationToken);

                //if (menu == null || !menu.Any())
                //    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idMenu}.");

                return menu;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idMenu}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idMenu}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do menu.", ex);
            }
        }

        public async Task<MenuRegraDto> MenuRegraServicoBuscaPorIdReferenciaAsync(int menuId, string regraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuRegra = await _repositorio.MenuRegraRepositorioBuscarPorIdReferenciaAsync(menuId, regraId, cancellationToken);

                if (menuRegra is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {regraId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuRegra;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {regraId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {regraId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task MenuRegraServicoRegistrarAsync(List<MenuRegraDto> menus, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menus == null || !menus.Any())
                    throw new ArgumentException("A lista de permissões está vazia ou nula.");

                await _repositorio.MenuRegistrarAsync(menus, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Repropaga a exceção para manter o controle de cancelamento
                throw;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"⚠️ Argumento inválido: {argEx.Message}");
                throw; // Pode ser tratado no controller
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao registrar permissões no serviço: {ex.Message}");
                throw new ApplicationException("Erro ao registrar permissões no serviço.", ex);
            }
        }

        public async Task MenuRegraServicoDeletetarAsync(MenuRegraDto menu, CancellationToken cancellationToken)
        {
            var idMenutintegracao = menu?.Id ?? 0;

            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu), "O objeto menu não pode ser nulo.");
            }

            if (idMenutintegracao <= 0)
            {
                throw new ArgumentException("ID inválido para exclusão da integração.");
            }

            try
            {
                await _repositorio.MenuRegraRepositorioDeletarAsync(menu, cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao excluir a integração com ID {idMenutintegracao}.", ex);
            }
        }

        #endregion

        #region Métodos com Referência entre Menu e Usuário

        public async Task MenuUsuarioServicoResgistrarAsync(List<MenuUsuarioDto> menus, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menus == null || !menus.Any())
                    throw new ArgumentException("A lista de permissões está vazia ou nula.");

                await _repositorio.MenuUsuarioRepositorioRegistrarAsync(menus, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Repropaga a exceção para manter o controle de cancelamento
                throw;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"⚠️ Argumento inválido: {argEx.Message}");
                throw; // Pode ser tratado no controller
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao registrar permissões no serviço: {ex.Message}");
                throw new ApplicationException("Erro ao registrar permissões no serviço.", ex);
            }
        }

        public async Task<List<MenuUsuarioDto>> MenuUsuarioServicoBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _repositorio.MenuUsuarioRepositorioBuscarPorIdMenuAsync(idMenu, cancellationToken);

                //if (menu == null || !menu.Any())
                //    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idMenu}.");

                return menu;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idMenu}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idMenu}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do menu.", ex);
            }
        }

        public async Task<AnyPointStoreMenuUsuario> MenuUsuarioServicoBuscaPorIdReferenciaAsync(int menuId, string usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuUsuario = await _repositorio.MenuUsuarioRepositorioBuscarPorIdReferenciaAsync(menuId, usuarioId, cancellationToken);

                if (menuUsuario is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {usuarioId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuUsuario;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {usuarioId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {usuarioId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task<List<AnyPointStoreMenuUsuario>> MenuUsuarioServicoBuscaPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _repositorio.MenuUsuarioRepositorioBuscarPorIdUsuariosync(idUsuario.ToString(), cancellationToken);

                if (menuIntegracao is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {idUsuario}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuIntegracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {idUsuario}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {idUsuario}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task MenuUsuarioServicoDeletarAsync(AnyPointStoreMenuUsuario menuUsuario, CancellationToken cancellationToken)
        {
            var idMenutintegracao = menuUsuario?.Id ?? 0;

            if (menuUsuario == null)
            {
                throw new ArgumentNullException(nameof(menuUsuario), "O objeto menu não pode ser nulo.");
            }

            if (idMenutintegracao <= 0)
            {
                throw new ArgumentException("ID inválido para exclusão da integração.");
            }

            try
            {
                await _repositorio.MenuUsuarioRepositorioDeletarAsync(menuUsuario, cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao excluir a integração com ID {idMenutintegracao}.", ex);
            }
        }

        #endregion

        #region Métodos com Referência entre Menu e Integração

        public async Task MenuIntegracaoServicoRegistrarAsync(List<MenuIntegracaoDto> menus, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menus == null || menus.Count == 0)
                    throw new ArgumentException("A lista de integrações não pode ser nula ou vazia.");

                await _repositorio.MenuIntegracaoRepositorioRegistrarAsync(menus, cancellationToken);

                Console.WriteLine($"✅ {menus.Count} integrações registradas com sucesso.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("⏳ Operação cancelada pelo cliente.");
                throw;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"⚠️ Erro de entrada: {argEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao registrar integrações: {ex.Message}");
                throw new ApplicationException("Erro ao registrar as integrações do menu.", ex);
            }
        }

        public async Task<List<MenuIntegracaoDto>> MenuIntegracaoServicoBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _repositorio.MenuIntegracaoRepositorioBuscarPorIdMenuAsync(idMenu, cancellationToken);

                //if (menu == null || !menu.Any())
                //    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idMenu}.");

                return menu;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idMenu}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw; // já está tratada acima com mensagem clara
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idMenu}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do menu.", ex);
            }
        }

        public async Task<AnyPointStoreMenuIntegracao> MenuIntegracaoServicoBuscaPorIdReferenciaAsync(int MenuId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _repositorio.MenuIntegracaoRepositorioBuscarPorIdReferenciaAsync(MenuId, IntegracaoId, cancellationToken);

                if (menuIntegracao is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {IntegracaoId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuIntegracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {IntegracaoId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {IntegracaoId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task MenuIntegracaoServicoDeletarAsync(AnyPointStoreMenuIntegracao menuIntegracao, CancellationToken cancellationToken)
        {
            var idMenutintegracao = menuIntegracao?.Id ?? 0;

            if (menuIntegracao == null)
            {
                throw new ArgumentNullException(nameof(menuIntegracao), "O objeto menu não pode ser nulo.");
            }

            if (idMenutintegracao <= 0)
            {
                throw new ArgumentException("ID inválido para exclusão da integração.");
            }

            try
            {
                await _repositorio.MenuIntegracaoRepositorioDeletarAsync(menuIntegracao, cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao excluir a integração com ID {idMenutintegracao}.", ex);
            }
        }

        #endregion

        #region Métodos Utilitários
        private async Task<ReferenciasMenuDto> PreencherReferenciasMenuAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var regrasVinculadas = await MenuRegraServicoBuscarPorIdMenuAsync(id, cancellationToken);
            var usuariosVinculados = await MenuUsuarioServicoBuscarPorIdMenuAsync(id, cancellationToken);
            var integracoesVinculadas = await MenuIntegracaoServicoBuscarPorIdMenuAsync(id, cancellationToken);
            var subMenu = await _repositorio.RepositorioObterSubMenusPorIdAsync(id, cancellationToken);

            return new ReferenciasMenuDto
            {
                RegrasVinculadas = regrasVinculadas,
                UsuariosVinculados = usuariosVinculados,
                IntegracoesVinculadas = integracoesVinculadas,
                SubMenus = subMenu
            };
        }

        #endregion
    }
}