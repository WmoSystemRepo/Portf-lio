using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Relatorios;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    /// <summary>
    /// Interface para comunicação com a API VExpenses.
    /// </summary>
    public interface IVExpensesApi
    {
        Task<(bool, string)> AlterarStatusAsync(AlteraStatus request, string uri, string token, CancellationToken cancellationToken);
        Task<(bool, string)> BuscarUsuarioPorIdAsync(string uri, string token, CancellationToken cancellationToken);

        /// <summary>
        /// Busca relatórios filtrados pelo status e filtros adicionais.
        /// </summary>
        /// <param name="status">status do relatório.</param>
        /// <param name="filtros">Objeto contendo os filtros de pesquisa.</param>
        /// <returns>Lista de relatórios encontrados.</returns>
        Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusAsync(string status, string uri, string token, FiltrosDto filtros, CancellationToken cancellationToken);
        Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusPagoAsync(string status, string uri, string token, CancellationToken cancellationToken);
        Task<(IReadOnlyList<IntegracaoVexpenseTitulosRelatoriosStatusDto> Reports, int TotalCount)> BuscarRelatoriosAsync(int pageNumber, int pageSize, string? status, string? search, CancellationToken cancellationToken);
        Task<ContagensRelatorios> ObterContagensRelatoriosAsync(CancellationToken cancellationToken);
        Task SaveChanges(List<ReportDto> reports, string status, CancellationToken cancellationToken);
    }
}