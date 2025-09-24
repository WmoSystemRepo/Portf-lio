using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;

namespace PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Menu
{
    public class MenuRepository : IMenuRepository
    {
        private readonly Context _context;

        public MenuRepository(Context context)
        {
            _context = context;
        }

        #region Repositório de Menu

        public async Task<List<MenuDto>> RepositorioListaMenusAsync(string userId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Busca apenas os menus, sem relacionamentos
                var menus = await _context.AnyPointStoreMenu
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                // Mapeia para DTO
                var result = menus.Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Nome = menu.Nome,
                    Rota = menu.Rota,
                    Icone = menu.Icone,
                    OrdenacaoMenu = menu.OrdenacaoMenu,
                    EhMenuPrincipal = menu.EhMenuPrincipal,
                    SubMenuReferenciaPrincipal = menu.SubMenuReferenciaPrincipal,
                    DataCriacao = menu.DataCriacao,
                    DataEdicao = menu.DataEdicao
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter menus.", ex);
            }
        }

        public async Task RepositorioRegistrarMenuAsync(MenuDto menuDto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(menuDto);
            cancellationToken.ThrowIfCancellationRequested();

            // Cria a entidade do menu a partir do DTO
            var menuEntity = new AnyPointStoreMenu
            {
                Nome = menuDto.Nome,
                Rota = menuDto.Rota,
                Icone = menuDto.Icone,
                OrdenacaoMenu = menuDto.OrdenacaoMenu,
                EhMenuPrincipal = menuDto.EhMenuPrincipal,
                SubMenuReferenciaPrincipal = menuDto.SubMenuReferenciaPrincipal,
                DataCriacao = menuDto.DataCriacao,
                DataEdicao = menuDto.DataCriacao
            };

            // Salva no banco
            await _context.AnyPointStoreMenu.AddAsync(menuEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<MenuDto?> RepositorioObterMenuPorIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _context.AnyPointStoreMenu
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            var response = new MenuDto
            {
                Id = result.Id,
                Nome = result.Nome,
                Rota = result.Rota,
                Icone = result.Icone,
                OrdenacaoMenu = result.OrdenacaoMenu,
                EhMenuPrincipal = result.EhMenuPrincipal,
                SubMenuReferenciaPrincipal = result.SubMenuReferenciaPrincipal,
                DataCriacao = result.DataCriacao,
                DataEdicao = result.DataEdicao
            };

            return response; // Fix: Add nullable annotation to the return type to handle possible null values.
        }

        public async Task RepositorioAtualizarMenuAsync(AnyPointStoreMenu menu, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(menu);

            var existing = await RepositorioObterMenuPorIdAsync(menu.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException("MEnu não encontrada!");

            cancellationToken.ThrowIfCancellationRequested();
            _context.AnyPointStoreMenu.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task RepositorioDeletarMenuAsync(MenuDto menu, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(menu);

            var menuEntity = new AnyPointStoreMenu
            {
                Id = menu.Id,
                Nome = menu.Nome,
                Rota = menu.Rota,
                Icone = menu.Icone,
                OrdenacaoMenu = menu.OrdenacaoMenu,
                EhMenuPrincipal = menu.EhMenuPrincipal,
                SubMenuReferenciaPrincipal = menu.SubMenuReferenciaPrincipal,
                DataCriacao = menu.DataCriacao,
                DataEdicao = menu.DataEdicao
            };
            cancellationToken.ThrowIfCancellationRequested();
            _context.AnyPointStoreMenu.Remove(menuEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MenuDto>> RepositorioObterSubMenusPorIdAsync(int idMenu, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _context.AnyPointStoreMenu
                .AsNoTracking()
                .Where(m => m.SubMenuReferenciaPrincipal == idMenu)
                .ToListAsync(cancellationToken);

            var response = result.Select(menu => new MenuDto
            {
                Id = menu.Id,
                Nome = menu.Nome,
                Rota = menu.Rota,
                Icone = menu.Icone,
                OrdenacaoMenu = menu.OrdenacaoMenu,
                EhMenuPrincipal = menu.EhMenuPrincipal,
                SubMenuReferenciaPrincipal = menu.SubMenuReferenciaPrincipal,
                DataCriacao = menu.DataCriacao,
                DataEdicao = menu.DataEdicao
            }).ToList();

            return response;
        }
        #endregion

        #region Repositório de Menu com Referência à Regra
        public async Task<List<MenuRegraDto>> MenuRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuRegra
                    .AsNoTracking()
                    .Where(m => m.MenuId == idMenu)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para MenuId {idMenu}.");
                    return new List<MenuRegraDto>();
                }

                var mapped = result.Select(entidade => new MenuRegraDto
                {
                    Id = entidade.Id,
                    MenuId = entidade.MenuId,
                    RegraId = entidade.RegraId,
                    Ativo = entidade.Ativo,
                    DataCriacao = entidade.DataCriacao,
                    DataEdicao = entidade.DataEdicao
                }).ToList();

                return mapped;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar integrações para MenuId {idMenu}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar integrações para MenuId {idMenu}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar integrações do menu {idMenu}.", ex);
            }
        }


        public async Task MenuRegistrarAsync(List<MenuRegraDto> menuDto, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(menuDto);
                cancellationToken.ThrowIfCancellationRequested();

                var entidade = menuDto.Select(menuDto => new AnyPointStoreMenuRegra
                {
                    MenuId = menuDto.MenuId,
                    RegraId = menuDto.RegraId,
                    Ativo = menuDto.Ativo,
                    DataCriacao = menuDto.DataCriacao,
                    DataEdicao = menuDto.DataEdicao
                }).ToList();

                await _context.AnyPointStoreMenuRegra.AddRangeAsync(entidade, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<MenuRegraDto>> MenuRepositorioListasAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuRegraDto> MenuRegraRepositorioBuscarPorIdReferenciaAsync(int menuId, string regraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuRegra
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.RegraId == regraId && m.MenuId == menuId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhuma regra encontrada com o ID {regraId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                var resultdto = new MenuRegraDto
                {
                    Id = result.Id,
                    MenuId = result.MenuId,
                    RegraId = result.RegraId,
                    Ativo = result.Ativo,
                    DataCriacao = result.DataCriacao,
                    DataEdicao = result.DataEdicao
                };

                return resultdto;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de integração ID: {regraId}");
                throw;
            }
        }

        public Task<List<MenuRegraDto>> MenuRepositorioEditarAsync(AnyPointStoreMenuRegra menuBD, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task MenuRegraRepositorioDeletarAsync(MenuRegraDto menu, CancellationToken cancellationToken)
        {
            if (menu == null || menu.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                var resultdto = new AnyPointStoreMenuRegra
                {
                    Id = (int)menu.Id,
                    MenuId = menu.MenuId,
                    RegraId = menu.RegraId,
                    Ativo = menu.Ativo,
                    DataCriacao = menu.DataCriacao,
                    DataEdicao = menu.DataEdicao
                };

                _context.AnyPointStoreMenuRegra.Remove(resultdto);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao excluir a integração do menu.", ex);
            }
        }

        #endregion

        #region Repositório de Menu com Referência à Usuário

        public async Task MenuUsuarioRepositorioRegistrarAsync(List<MenuUsuarioDto> menus, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(menus);
            cancellationToken.ThrowIfCancellationRequested();

            var entidades = menus.Select(menuDto => new AnyPointStoreMenuUsuario
            {
                MenuId = menuDto.MenuId,
                UsuarioId = menuDto.UsuarioId,
                Ativo = menuDto.Ativo,
                DataCriacao = menuDto.DataCriacao,
                DataEdicao = menuDto.DataEdicao
            }).ToList();

            await _context.AnyPointStoreMenuUsuario.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();
        }

        public async Task<AnyPointStoreMenuUsuario> MenuUsuarioRepositorioBuscarPorIdReferenciaAsync(int menuId, string usuarioId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuUsuario
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.MenuId == menuId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhum usuário encontrado com o ID {usuarioId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de Usuário ID: {usuarioId}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar a integração com ID {usuarioId}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task<List<AnyPointStoreMenuUsuario>> MenuUsuarioRepositorioBuscarPorIdUsuariosync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuUsuario
                    .AsNoTracking()
                    .Where(m => m.UsuarioId == idUsuario)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    var msg = $"❗ Nenhum menu encontrado para o usuário com ID {idUsuario}.";
                    Console.WriteLine(msg);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de menus do Usuário ID: {idUsuario}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar menus para o usuário com ID {idUsuario}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task<List<MenuUsuarioDto>> MenuUsuarioRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuUsuario
                    .AsNoTracking()
                    .Where(m => m.MenuId == idMenu)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para MenuId {idMenu}.");
                    return new List<MenuUsuarioDto>();
                }

                var mapped = result.Select(entidade => new MenuUsuarioDto
                {
                    Id = entidade.Id,
                    MenuId = entidade.MenuId,
                    UsuarioId = entidade.UsuarioId,
                    Ativo = entidade.Ativo,
                    DataCriacao = entidade.DataCriacao,
                    DataEdicao = entidade.DataEdicao
                }).ToList();

                return mapped;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar integrações para MenuId {idMenu}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar integrações para MenuId {idMenu}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar integrações do menu {idMenu}.", ex);
            }
        }

        public async Task MenuUsuarioRepositorioDeletarAsync(AnyPointStoreMenuUsuario menuUsuario, CancellationToken cancellationToken)
        {
            if (menuUsuario == null || menuUsuario.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão de usuário.");
            }

            try
            {
                _context.AnyPointStoreMenuUsuario.Remove(menuUsuario);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir o usuário do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao excluir o usuário do menu.", ex);
            }
        }

        #endregion

        #region Repositório de Menu com Referência à Integração

        public async Task MenuIntegracaoRepositorioRegistrarAsync(List<MenuIntegracaoDto> menusDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ArgumentNullException.ThrowIfNull(menusDto, nameof(menusDto));

                if (!menusDto.Any())
                    throw new ArgumentException("A lista de integrações está vazia.", nameof(menusDto));

                var entidades = menusDto.Select(menuDto => new AnyPointStoreMenuIntegracao
                {
                    MenuId = menuDto.MenuId,
                    IntegracaoId = menuDto.IntegracaoId,
                    Ativo = menuDto.Ativo,
                    DataCriacao = menuDto.DataCriacao,
                    DataEdicao = menuDto.DataEdicao
                }).ToList();

                await _context.AnyPointStoreMenuIntegracao.AddRangeAsync(entidades, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                Console.WriteLine($"✅ {entidades.Count} integrações persistidas com sucesso.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("⏳ Operação cancelada durante persistência de integrações.");
                throw;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"⚠️ Erro de argumento: {argEx.Message}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"❌ Erro ao salvar no banco de dados: {dbEx.InnerException?.Message ?? dbEx.Message}");
                throw new Exception("Erro ao salvar integrações no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao registrar integrações: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao registrar integrações.", ex);
            }
        }

        public async Task<AnyPointStoreMenuIntegracao> MenuIntegracaoRepositorioBuscarPorIdReferenciaAsync(int MenuId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuIntegracao
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.MenuId == MenuId && m.IntegracaoId == IntegracaoId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhuma integração encontrada com o ID {IntegracaoId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de integração ID: {IntegracaoId}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar a integração com ID {IntegracaoId}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task<List<MenuIntegracaoDto>?> MenuIntegracaoRepositorioBuscarPorIdMenuAsync(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMenuIntegracao
                    .AsNoTracking()
                    .Where(m => m.MenuId == idMenu)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para MenuId {idMenu}.");
                    return new List<MenuIntegracaoDto>();
                }

                var mapped = result.Select(entidade => new MenuIntegracaoDto
                {
                    Id = entidade.Id,
                    MenuId = entidade.MenuId,
                    IntegracaoId = entidade.IntegracaoId,
                    Ativo = entidade.Ativo,
                    DataCriacao = entidade.DataCriacao,
                    DataEdicao = entidade.DataEdicao
                }).ToList();

                return mapped;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar integrações para MenuId {idMenu}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar integrações para MenuId {idMenu}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar integrações do menu {idMenu}.", ex);
            }
        }

        public async Task MenuIntegracaoRepositorioDeletarAsync(AnyPointStoreMenuIntegracao menuIntegracao, CancellationToken cancellationToken)
        {
            if (menuIntegracao == null || menuIntegracao.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                _context.AnyPointStoreMenuIntegracao.Remove(menuIntegracao);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                // Erro específico do EF ao tentar atualizar/excluir no banco
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                // Erro genérico
                throw new Exception("Erro inesperado ao excluir a integração do menu.", ex);
            }
        }

        #endregion
    }
}