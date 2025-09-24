using Hangfire;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.Bacen.Jobs
{
    public class CambioJobService
    {
        private readonly CotacaoService _cotacaoService;

        public CambioJobService(CotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task ExecutarCotacaoDiariaAsync(CancellationToken cancellationToken)
        {
            await _cotacaoService.ObterCotacoesPorPeriodoAsync("USD", DateTime.Now, DateTime.Now, cancellationToken);
        }
    }

}
