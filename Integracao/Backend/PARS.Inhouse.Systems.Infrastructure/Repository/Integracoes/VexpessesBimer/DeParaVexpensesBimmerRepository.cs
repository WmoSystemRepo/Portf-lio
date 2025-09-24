using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento;
using System.Linq.Expressions;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer
{
    public class DeParaVexpensesBimmerRepository : IGestaoMepeamentoComposRepository
    {
        private readonly Context _context;

        public DeParaVexpensesBimmerRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnyPointDeparas>> ListarMapeamentoCamposAsync(CancellationToken cancellationToken)
        {
            return await _context.AnyPointDeparas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AnyPointDeparas?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AnyPointDeparas
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<AnyPointDeparas?> GetDeparaAsync(Expression<Func<AnyPointDeparas, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.AnyPointDeparas.FirstOrDefaultAsync(predicate);
        }

        public async Task NovoRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);

                _context.AnyPointDeparas.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task EditarRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var existingEntity = await _context.AnyPointDeparas
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == entity.Id);

            if (existingEntity == null)
                throw new KeyNotFoundException("Registro não encontrado!");

            _context.AnyPointDeparas.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirRegistroAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.AnyPointDeparas.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException("Registro não encontrado!");

            _context.AnyPointDeparas.Remove(entity);
            await _context.SaveChangesAsync();
        }



        public async Task MapeamentoIntegracaoRepositorioRegistrarAsync(List<MapeamentoIntegracaoDto> mapeamentoIntegracaoDto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ArgumentNullException.ThrowIfNull(mapeamentoIntegracaoDto, nameof(mapeamentoIntegracaoDto));

                if (!mapeamentoIntegracaoDto.Any())
                    throw new ArgumentException("A lista de integrações está vazia.", nameof(mapeamentoIntegracaoDto));

                var entidades = mapeamentoIntegracaoDto.Select(mapeamentoIntegracao => new AnyPointStoreMapeamentoIntegracao
                {
                    MapeamentoId = mapeamentoIntegracao.MapeamentoId,
                    IntegracaoId = mapeamentoIntegracao.IntegracaoId,
                    Ativo = mapeamentoIntegracao.Ativo,
                    DataCriacao = mapeamentoIntegracao.DataCriacao,
                    DataEdicao = mapeamentoIntegracao.DataEdicao
                }).ToList();

                await _context.AnyPointStoreMapeamentoIntegracao.AddRangeAsync(entidades, cancellationToken);
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

        public async Task<List<AnyPointStoreMapeamentoIntegracao>?> MapeamentoIntegracaoRepositorioBuscarPorIdMapeamentoAsync(int idMapeamento, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMapeamentoIntegracao
                    .AsNoTracking()
                    .Where(m => m.MapeamentoId == idMapeamento)
                    .ToListAsync(cancellationToken);

                if (result == null || result.Count == 0)
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para MenuId {idMapeamento}.");
                    return new List<AnyPointStoreMapeamentoIntegracao>(); // Return an empty list instead of null
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada ao buscar integrações para MenuId {idMapeamento}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao buscar integrações para MenuId {idMapeamento}: {ex.Message}");
                throw new ApplicationException($"Erro inesperado ao buscar integrações do menu {idMapeamento}.", ex);
            }
        }

        public async Task<AnyPointStoreMapeamentoIntegracao> MapeamentoIntegracaoRepositorioBuscarPorIdReferenciaAsync(int mapeamentoId, int integracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await _context.AnyPointStoreMapeamentoIntegracao
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.MapeamentoId == mapeamentoId && m.IntegracaoId == integracaoId, cancellationToken);

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

        public async Task MapeamentoIntegracaoRepositorioDeletarAsync(AnyPointStoreMapeamentoIntegracao mapeamentoIntegracao, CancellationToken cancellationToken)
        {
            if (mapeamentoIntegracao == null || mapeamentoIntegracao.Id <= 0)
            {
                throw new ArgumentException("Dados inválidos para exclusão da integração.");
            }

            try
            {
                _context.AnyPointStoreMapeamentoIntegracao.Remove(mapeamentoIntegracao);
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
    }
}