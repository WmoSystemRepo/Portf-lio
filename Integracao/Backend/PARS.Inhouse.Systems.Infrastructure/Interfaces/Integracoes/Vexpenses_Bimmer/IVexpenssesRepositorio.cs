using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IVexpenssesRepositorio
    {
        Task<IReadOnlyList<IntegracaoBimmerInsertOK>> RecuperarTitulosEnviadosComSucessoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IntegracaoBimmerHistoricoErrosInsercoes>> RecuperarHistoricoErrosIntegracaoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
        Task<List<IntegracaoVexpenseTitulosRelatoriosStatus>> ObterTitulosAprovadosAsync(CancellationToken cancellationToken = default);
        Task<IntegracaoVexpenseTitulosRelatoriosStatus?> ObterTituloAprovadoPorIdAsync(int idResponse, CancellationToken cancellationToken = default);
        Task<IntegracaoBimmerInsercaoPendentes?> ObterPendentePorIdResponseAsync(int idResponse, CancellationToken cancellationToken = default);
        Task<List<IntegracaoBimmerInsercaoPendentes>?> RecuperarTodosRegistrosPendentesAsync(CancellationToken cancellationToken = default);
        Task<IntegracaoBimmerInsertOK> RecuperarTituloPagoPorIdAsync(int tituloId, CancellationToken cancellationToken = default);
        Task<AnyPointDeparas?> RecuperarNaturezaLancamentoVexpenseAsync(string? paymentMethodId, CancellationToken cancellationToken = default);
        Task<AnyPointDeparas?> RecuperarMapeamentoCentroDeCustoAsync(string? payingCompanyId, CancellationToken cancellationToken = default);
        Task<AnyPointDeparas?> RecuperarDeParaEmpresaPagadoraAsync(string? payingCompanyId, CancellationToken cancellationToken = default);
        Task RegistrarTituloPagoAsync(IntegracaoBimmerInsertOK tituloPago, CancellationToken cancellationToken = default);
        Task RegistrarHistoricoInsercaoComErroAsync(IntegracaoBimmerHistoricoErrosInsercoes historico, CancellationToken cancellationToken = default);
        Task SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async(CancellationToken cancellationToken = default);
        Task<List<int>> ExcluirPendenciasPorIdsAsync(int[] ids, CancellationToken cancellationToken = default);
    }
}