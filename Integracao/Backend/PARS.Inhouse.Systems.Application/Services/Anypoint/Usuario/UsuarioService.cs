using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint.Usuario
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repositorio;

        public UsuarioService(IUsuarioRepository repositorio)
        {
            _repositorio = repositorio;
        }

        #region Métodos com Referência entre Usuário e Integração

        public async Task UsuarioIntegracaoServicoRegistrarAsync(List<UsuarioIntegracaoDto> menus, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menus == null || menus.Count == 0)
                    throw new ArgumentException("A lista de integrações não pode ser nula ou vazia.");

                await _repositorio.UsuarioIntegracaoRepositorioRegistrarAsync(menus, cancellationToken);

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
                throw new ApplicationException("Erro ao registrar as integrações do usuario.", ex);
            }
        }

        public async Task<List<AnyPointStoreUsuarioIntegracao>> UsuarioIntegracaoServicoBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _repositorio.UsuarioIntegracaoRepositorioBuscarPorIdMenuAsync(idUsuario, cancellationToken);

                if (menu == null || !menu.Any())
                    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idUsuario}.");

                return menu;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idUsuario}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idUsuario}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do usuario.", ex);
            }
        }

        public async Task<AnyPointStoreUsuarioIntegracao> UsuarioIntegracaoServicoBuscaPorIdReferenciaAsync(string usuarioId, int integracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _repositorio.UsuarioIntegracaoRepositorioBuscarPorIdReferenciaAsync(usuarioId, integracaoId, cancellationToken);

                if (menuIntegracao is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {integracaoId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuIntegracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {integracaoId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {integracaoId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task UsuarioIntegracaoServicoExcluirAsync(AnyPointStoreUsuarioIntegracao menuIntegracao, CancellationToken cancellationToken)
        {
            var idMenutintegracao = menuIntegracao?.Id ?? 0;

            if (menuIntegracao == null)
            {
                throw new ArgumentNullException(nameof(menuIntegracao), "O objeto usuario não pode ser nulo.");
            }

            if (idMenutintegracao <= 0)
            {
                throw new ArgumentException("ID inválido para exclusão da integração.");
            }

            try
            {
                await _repositorio.UsuarioIntegracaoRepositorioExcluirAsync(menuIntegracao, cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do usuario no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao excluir a integração com ID {idMenutintegracao}.", ex);
            }
        }

        #endregion

        #region Métodos com Referência entre Usuário e Permissão

        public async Task UsuarioPermissaoServicoRegistrarAsync(List<UsuarioPermissaoDto> permissao, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (permissao == null || permissao.Count == 0)
                    throw new ArgumentException("A lista de permissões não pode ser nula ou vazia.");

                await _repositorio.UsuarioPermissaoRepositorioRegistrarAsync(permissao, cancellationToken);

                Console.WriteLine($"✅ {permissao.Count} permissão registradas com sucesso.");
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
                Console.WriteLine($"❌ Erro inesperado ao registrar permissões: {ex.Message}");
                throw new ApplicationException("Erro ao registrar as permissões do usuario.", ex);
            }
        }

        public async Task<UsuarioPermissaoDto> UsuarioPermissaoServicoBuscaPorIdReferenciaAsync(string usuarioId, int permissaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _repositorio.UsuarioPermissaoRepositorioBuscarPorIdReferenciaAsync(usuarioId, permissaoId, cancellationToken);

                if (menuIntegracao is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {permissaoId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return menuIntegracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {permissaoId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {permissaoId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task<List<UsuarioPermissaoDto>> UsuarioPermissaoServicoBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _repositorio.UsuarioPermissaoRepositorioBuscarPorIdUsuarioAsync(idUsuario, cancellationToken);

                if (menu == null || !menu.Any())
                    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idUsuario}.");

                return menu;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idUsuario}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idUsuario}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do usuario.", ex);
            }
        }

        public async Task UsuarioPermissaoServicoExcluirAsync(UsuarioPermissaoDto usuarioPermissao, CancellationToken cancellationToken)
        {
            if (usuarioPermissao == null)
            {
                throw new ArgumentNullException(nameof(usuarioPermissao), "O objeto usuario não pode ser nulo.");
            }
            cancellationToken.ThrowIfCancellationRequested();

            await _repositorio.UsuarioPermissaoRepositorioExcluirAsync(usuarioPermissao, cancellationToken);
        }

        #endregion

        #region Métodos com Referência entre Usuário e Regra

        public async Task UsuarioRegraServicoRegistrarAsync(List<UsuarioRegraDto> regra, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (regra == null || regra.Count == 0)
                    throw new ArgumentException("A lista de permissões não pode ser nula ou vazia.");

                await _repositorio.UsuarioRegraRepositorioRegistrarAsync(regra, cancellationToken);

                Console.WriteLine($"✅ {regra.Count} permissão registradas com sucesso.");
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
                Console.WriteLine($"❌ Erro inesperado ao registrar permissões: {ex.Message}");
                throw new ApplicationException("Erro ao registrar as permissões do usuario.", ex);
            }
        }

        public async Task<UsuarioRegraDto> UsuarioRegraServicoBuscaPorIdReferenciaAsync(string usuarioId, string regraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var usuarioRegra = await _repositorio.UsuarioRegraRepositorioBuscarPorIdReferenciaAsync(usuarioId, regraId, cancellationToken);

                if (usuarioRegra is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {regraId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return usuarioRegra;
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

        public async Task<List<UsuarioRegraDto>> UsuarioRegraServicoBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var usuario = await _repositorio.UsuarioRegraRepositorioBuscarPorIdUsuarioAsync(idUsuario, cancellationToken);

                if (usuario == null || !usuario.Any())
                    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idUsuario}.");

                return usuario;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idUsuario}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idUsuario}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do usuario.", ex);
            }
        }

        public async Task UsuarioRegraServicoExcluirAsync(UsuarioRegraDto usuarioRegra, CancellationToken cancellationToken)
        {
            if (usuarioRegra == null)
            {
                throw new ArgumentNullException(nameof(usuarioRegra), "O objeto usuario não pode ser nulo.");
            }
            cancellationToken.ThrowIfCancellationRequested();

            await _repositorio.UsuarioRegraRepositorioExcluirAsync(usuarioRegra, cancellationToken);
        }

        #endregion
    }
}
