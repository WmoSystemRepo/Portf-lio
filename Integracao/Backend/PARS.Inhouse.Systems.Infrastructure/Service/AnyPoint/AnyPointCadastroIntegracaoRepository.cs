using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint;

namespace PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint
{
    public class AnyPointCadastroIntegracaoRepository : IAnyPointCadastroIntegracaoRepository
    {
        private readonly Context _context;

        public AnyPointCadastroIntegracaoRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnyPointStoreGestaoIntegracao>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.AnyPointStoreGestaoIntegracao
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao recuperar todas as integrações.", ex);
            }
        }

        public async Task<AnyPointStoreGestaoIntegracao?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AnyPointStoreGestaoIntegracao
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(AnyPointStoreGestaoIntegracao integracao, CancellationToken cancellationToken)
        {
            await _context.AnyPointStoreGestaoIntegracao.AddAsync(integracao, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(AnyPointStoreGestaoIntegracao integracao, CancellationToken cancellationToken)
        {
            _context.AnyPointStoreGestaoIntegracao.Update(integracao);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var integracao = await _context.AnyPointStoreGestaoIntegracao
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (integracao != null)
            {
                _context.AnyPointStoreGestaoIntegracao.Remove(integracao);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
