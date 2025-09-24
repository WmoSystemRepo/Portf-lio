using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Relatorios;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce
{
    public interface IVExpensesService
    {
        Task<(bool, string)> AlterarStatusAsync(int id, AlteraStatus request, CancellationToken cancellationToken = default);
        Task<(bool, string)> BuscarUsuarioPorIdVexpenssesAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ReportDto>> ObterRelatorioPorStatusVexpenssesAsync(string status, FiltrosDto filtrosDto, List<IntegracaoBimmerInsercaoPendentes>? listaPendencias = null, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<IntegracaoVexpenseTitulosRelatoriosStatusDto> Reports, int TotalCount)> BuscarRelatoriosAsync(int pageNumber, int pageSize, string? status, string? search, CancellationToken cancellationToken = default);
        Task<ContagensRelatorios> ObterContagensRelatoriosAsync(CancellationToken cancellationToken = default);
    }
}