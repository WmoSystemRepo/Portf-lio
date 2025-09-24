using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Bacen;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen.ErrosIntegracoes
{
    /// <summary>
    /// Responsável por persistir e consultar logs de erro da integração Bacen → Cotação no SQL.
    /// Essa classe é específica desta integração.
    /// </summary>
    public class IntegracaoBacenLogErroRepository : IIntegracaoBacenLogErroRepository
    {
        private readonly Context _context;

        public IntegracaoBacenLogErroRepository(Context context)
        {
            _context = context;
        }

        public async Task SaveAsync(IntegracaoBacenLogErros log, CancellationToken ct = default)
        {
            _context.Set<IntegracaoBacenLogErros>().Add(log);
            await _context.SaveChangesAsync(ct);
        }
    }
}