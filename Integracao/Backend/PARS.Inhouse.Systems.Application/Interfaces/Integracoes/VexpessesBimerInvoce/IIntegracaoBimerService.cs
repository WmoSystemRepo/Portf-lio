using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Bimer;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce
{
    public interface IIntegracaoBimerService
    {
        Task<TitlePayResponseDto?> RegistrarTituloAPagarNoBimmerAsync(BimmerRequestRequiredFieldsDto bimerRequestDto, string token, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> AutenticarNoBimmerAsync(AuthRequestDto request, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RenovarAutenticacaoNoBimmerAsync(ReauthenticateRequestDto request, CancellationToken cancellationToken = default);
        Task<PessoaResponseDto> ObterBimerIdentificadorPessoaPorCpfAsync(string cpf, string token, CancellationToken cancellationToken = default);
        Task<List<(int, string, bool)>> ProcessarEnvioTitulosParaBimmerAsync(string token, bool envioManual, int[]? idsTitulos = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IntegracaoBimmerInsercaoPendentes>> ObterPendenciasDeIntegracaoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IntegracaoBimmerHistoricoErrosInsercoes>> ObterHistoricoErrosDeIntegracaoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
        Task<List<IntegracaoBimmerInsercaoPendentes>?> ObterTodosRrgistrosTitulosPendentesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IntegracaoBimmerInsertOK>> ObterInsercoesBemSucedidasDeIntegracaoComPaginamentoAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
        Task<int> ExcluirPendenciasAsync(int[] ids, CancellationToken cancellationToken);
    }
}
