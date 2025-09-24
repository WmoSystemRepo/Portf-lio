namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce
{
    public interface IMonitorSppBimerInvoceRepositorio
    {
        Task<IEnumerable<IntegracaoSppBimerInvoce>> BuscarFiltrosAsync(
            string? status, DateTime? dataInicio, DateTime? dataFim, CancellationToken ct);

        Task<IEnumerable<IntegracaoSppBimerInvoce>> ListarTodosAsync(CancellationToken ct);

        Task<IntegracaoSppBimerInvoce?> ObterPorIdAsync(int id, CancellationToken ct);

        Task AtualizarAsync(IntegracaoSppBimerInvoce entidade, CancellationToken ct);

        Task<string> ReprocessarAsync(ReprocessarBimerRequestDto entidade, CancellationToken ct);
    }
}
