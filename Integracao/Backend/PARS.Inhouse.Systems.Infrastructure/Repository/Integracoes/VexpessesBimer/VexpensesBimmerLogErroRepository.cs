using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer
{
    public class VexpensesBimmerLogErroRepository
    {
        private readonly Context _context;

        public VexpensesBimmerLogErroRepository(Context context)
        {
            _context = context;
        }

        public async Task SalvarAsync(IntegracaoVexpenssesBimmerLogErros log)
        {
            _context.IntegracaoVexpenssesBimmerLogErros.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}