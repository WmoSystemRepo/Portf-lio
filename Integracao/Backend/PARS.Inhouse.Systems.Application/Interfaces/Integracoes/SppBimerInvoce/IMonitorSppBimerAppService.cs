using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoice
{
    public interface IMonitorSppBimerAppService
    {
        Task<IEnumerable<MonitoramentoSppBimerInvoceDto>> ListarTodosAsync(CancellationToken ct);

        Task<IEnumerable<MonitoramentoSppBimerInvoceDto>> ObterMonitoramentosAsync(
            string? status,
            DateTime? dataInicio,
            DateTime? dataFim,
            CancellationToken ct);

        Task<string> ReprocessarAsync(ReprocessarBimerRequestDto request, CancellationToken ct);
    }
}
