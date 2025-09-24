using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using System.Linq.Expressions;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes
{
    /// <summary>
    /// Responsável por persistir e consultar logs de erro da integração Vexpenses → Bimmer no SQL Server.
    /// Essa classe é específica desta integração.
    /// </summary>
    public class IntegracaoVexpensesBimmerLogErroRepository : IIntegracaoVexpensesBimmerLogErroRepository
    {
        private readonly Context _context;

        public IntegracaoVexpensesBimmerLogErroRepository(Context context)
        {
            _context = context;
        }

        public async Task SaveAsync(IntegracaoVexpenssesBimmerLogErros log, CancellationToken ct = default)
        {
            _context.Set<IntegracaoVexpenssesBimmerLogErros>().Add(log);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<IntegracaoVexpenssesBimmerLogErros>> GetPendingMigrationAsync(DateTime cutoff, CancellationToken ct = default)
        {
            return await _context.Set<IntegracaoVexpenssesBimmerLogErros>()
                .Where(x => x.DataHoraUtc < cutoff)
                .ToListAsync(ct);
        }

        public async Task MarkAsMigratedAsync(long id, CancellationToken ct = default)
        {
            var log = await _context.Set<IntegracaoVexpenssesBimmerLogErros>()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (log != null)
            {
                _context.Update(log);
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task<List<IntegracaoVexpenssesBimmerLogErros>> GetPaginatedAsync(int pageNumber, int pageSize, string? search, CancellationToken ct = default)
        {
            var query = _context.Set<IntegracaoVexpenssesBimmerLogErros>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.Endpoint, $"%{search}%") ||
                    EF.Functions.Like(x.Message, $"%{search}%") ||
                    EF.Functions.Like(x.Payload, $"%{search}%"));
            }

            return await query
                .OrderByDescending(x => x.DataHoraUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountDocumentsAsync(Expression<Func<IntegracaoVexpenssesBimmerLogErros, bool>> filter, CancellationToken ct = default)
        {
            return await _context.Set<IntegracaoVexpenssesBimmerLogErros>()
                .CountAsync(filter, ct);
        }

        public async Task<List<IntegracaoVexpenssesBimmerLogErros>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<IntegracaoVexpenssesBimmerLogErros>().ToListAsync(ct);
        }
    }
}
