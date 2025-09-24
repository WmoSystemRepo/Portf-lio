using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce
{
    public sealed class IntegracaoSppBimerInvoceLogErrosRepository : IIntegracaoSppBimerInvoceLogErrosRepository
    {
        private readonly Context _context;

        public IntegracaoSppBimerInvoceLogErrosRepository(Context context)
        {
            _context = context;
        }

        public async Task SaveAsync(IntegracaoSppBimerInvoceLogErros log, CancellationToken ct = default)
        {
            _context.Set<IntegracaoSppBimerInvoceLogErros>().Add(log);
            await _context.SaveChangesAsync(ct);
        }
    }
}
