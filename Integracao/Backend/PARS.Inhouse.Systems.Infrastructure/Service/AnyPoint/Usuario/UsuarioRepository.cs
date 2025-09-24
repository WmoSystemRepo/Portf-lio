using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Usuario
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Context _context;

        public UsuarioRepository(Context context)
        {
            _context = context;
        }

        #region Repositório de Menu com Referência à Regra

        public async Task UsuarioRegraRepositorioRegistrarAsync(List<UsuarioRegraDto> usuario, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            cancellationToken.ThrowIfCancellationRequested();

            var entidades = usuario.Select(menuDto => new AnyPointStoreUsuarioRegra
            {
                UsuarioId = menuDto.UsuarioId,
                RegraId = menuDto.RegraId,
                Ativo = menuDto.Ativo,
                DataCriacao = menuDto.DataCriacao,
                DataEdicao = menuDto.DataEdicao
            }).ToList();

            await _context.AnyPointStoreUsuarioRegra.AddRangeAsync(entidades, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UsuarioRegraDto>> UsuarioRegraRepositorioBuscarPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioRegra
                    .AsNoTracking()
                    .Where(m => m.UsuarioId == idUsuario)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma Regra encontrada para UsuarioId {idUsuario}.");
                    return null;
                }

                var listDto = result.Select(x => new UsuarioRegraDto
                {
                    Id = x.Id,
                    UsuarioId = x.UsuarioId,
                    RegraId = x.RegraId,
                    DataCriacao = x.DataCriacao,
                    DataEdicao = x.DataEdicao,
                    Ativo = x.Ativo
                }).ToList();

                return listDto;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar permissões para UsuarioId {idUsuario}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar permissões para UsuarioId {idUsuario}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar permissões do usuário {idUsuario}.", ex);
            }
        }

        public async Task<UsuarioRegraDto> UsuarioRegraRepositorioBuscarPorIdReferenciaAsync(string usuarioId, string regraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioRegra
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.RegraId == regraId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhuma integração encontrada com o ID {regraId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                var response = new UsuarioRegraDto
                {
                    Id = result.Id,
                    UsuarioId = result.UsuarioId,
                    RegraId = result.RegraId,
                    DataCriacao = result.DataCriacao,
                    DataEdicao = result.DataEdicao,
                    Ativo = result.Ativo
                };

                return response;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de integração ID: {regraId}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar a integração com ID {regraId}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task UsuarioRegraRepositorioExcluirAsync(UsuarioRegraDto usuarioRegra, CancellationToken cancellationToken)
        {
            if (usuarioRegra == null || usuarioRegra.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                var usuarioEntity = new AnyPointStoreUsuarioRegra
                {
                    Id = usuarioRegra.Id,
                    UsuarioId = usuarioRegra.UsuarioId,
                    RegraId = usuarioRegra.RegraId,
                    Ativo = usuarioRegra.Ativo,
                    DataCriacao = usuarioRegra.DataCriacao,
                    DataEdicao = usuarioRegra.DataEdicao
                };

                _context.AnyPointStoreUsuarioRegra.Remove(usuarioEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                // Erro específico do EF ao tentar atualizar/excluir no banco
                throw new InvalidOperationException("Erro ao tentar excluir a regra do usuário no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                // Erro genérico
                throw new Exception("Erro inesperado ao excluir a regra do usuário.", ex);
            }
        }

        #endregion

        #region Repositório de Menu com Referência à Permissão

        public async Task UsuarioPermissaoRepositorioRegistrarAsync(List<UsuarioPermissaoDto> usuario, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            cancellationToken.ThrowIfCancellationRequested();

            var entidades = usuario.Select(menuDto => new AnyPointStoreUsuarioPermissao
            {
                UsuarioId = menuDto.UsuarioId,
                PermissaoId = menuDto.PermissaoId,
                Ativo = menuDto.Ativo,
                DataCriacao = menuDto.DataCriacao,
                DataEdicao = menuDto.DataEdicao
            }).ToList();

            await _context.AnyPointStoreUsuarioPermissao.AddRangeAsync(entidades, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UsuarioPermissaoDto>> UsuarioPermissaoRepositorioBuscarPorIdUsuarioAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioPermissao
                    .AsNoTracking()
                    .Where(m => m.UsuarioId == idUsuario)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma permissão encontrada para UsuarioId {idUsuario}.");
                    return null;
                }

                var listDto = result.Select(x => new UsuarioPermissaoDto
                {
                    Id = x.Id,
                    UsuarioId = x.UsuarioId,
                    PermissaoId = x.PermissaoId,
                    DataCriacao = x.DataCriacao,
                    DataEdicao = x.DataEdicao,
                    Ativo = x.Ativo
                }).ToList();

                return listDto;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar permissões para UsuarioId {idUsuario}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar permissões para UsuarioId {idUsuario}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar permissões do usuário {idUsuario}.", ex);
            }
        }

        public async Task<UsuarioPermissaoDto> UsuarioPermissaoRepositorioBuscarPorIdReferenciaAsync(string usuarioId, int permissaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioPermissao
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.PermissaoId == permissaoId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhuma integração encontrada com o ID {permissaoId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                var response = new UsuarioPermissaoDto
                {
                    Id = result.Id,
                    UsuarioId = result.UsuarioId,
                    PermissaoId = result.PermissaoId,
                    DataCriacao = result.DataCriacao,
                    Ativo = result.Ativo
                };

                return response;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de integração ID: {permissaoId}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar a integração com ID {permissaoId}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task UsuarioPermissaoRepositorioExcluirAsync(UsuarioPermissaoDto usuarioPermissao, CancellationToken cancellationToken)
        {

            if (usuarioPermissao == null || usuarioPermissao.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                var entity = new AnyPointStoreUsuarioPermissao
                {
                    Id = usuarioPermissao.Id,
                    UsuarioId = usuarioPermissao.UsuarioId,
                    PermissaoId = usuarioPermissao.PermissaoId,
                    Ativo = usuarioPermissao.Ativo,
                    DataCriacao = usuarioPermissao.DataCriacao,
                    DataEdicao = usuarioPermissao.DataEdicao
                };

                _context.AnyPointStoreUsuarioPermissao.Remove(entity);
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

        #region Repositório de Menu com Referência à Integração

        public async Task UsuarioIntegracaoRepositorioRegistrarAsync(List<UsuarioIntegracaoDto> usuario, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            cancellationToken.ThrowIfCancellationRequested();

            var entidades = usuario.Select(menuDto => new AnyPointStoreUsuarioIntegracao
            {
                UsuarioId = menuDto.UsuarioId,
                IntegracaoId = menuDto.IntegracaoId,
                Ativo = menuDto.Ativo,
                DataCriacao = menuDto.DataCriacao,
                DataEdicao = menuDto.DataEdicao
            }).ToList();

            await _context.AnyPointStoreUsuarioIntegracao.AddRangeAsync(entidades, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<AnyPointStoreUsuarioIntegracao>?> UsuarioIntegracaoRepositorioBuscarPorIdMenuAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioIntegracao
                    .AsNoTracking()
                    .Where(m => m.UsuarioId == idUsuario)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para MenuId {idUsuario}.");
                    return null;
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar integrações para MenuId {idUsuario}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar integrações para MenuId {idUsuario}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar integrações do menu {idUsuario}.", ex);
            }
        }

        public async Task<AnyPointStoreUsuarioIntegracao> UsuarioIntegracaoRepositorioBuscarPorIdReferenciaAsync(string usuarioId, int integracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreUsuarioIntegracao
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UsuarioId == usuarioId && m.IntegracaoId == integracaoId, cancellationToken);

                if (result is null)
                {
                    var msg = $"❗ Nenhuma integração encontrada com o ID {integracaoId}.";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada durante busca de integração ID: {integracaoId}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var msg = $"❌ Erro inesperado ao consultar a integração com ID {integracaoId}.";
                Console.WriteLine($"{msg} Detalhes: {ex.Message}");
                throw new ApplicationException(msg, ex);
            }
        }

        public async Task UsuarioIntegracaoRepositorioExcluirAsync(AnyPointStoreUsuarioIntegracao usuarioEntidade, CancellationToken cancellationToken)
        {

            if (usuarioEntidade == null || usuarioEntidade.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                _context.AnyPointStoreUsuarioIntegracao.Remove(usuarioEntidade);
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