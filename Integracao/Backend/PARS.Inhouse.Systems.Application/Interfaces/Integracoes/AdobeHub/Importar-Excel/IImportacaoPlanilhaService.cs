using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Importar_Excel
{
    public interface IImportacaoPlanilhaService
    {
        Task<List<PlaninhasImportadosDto>> ListaPlaninhasImportadasAsync(CancellationToken cancellationToken);
        Task<PlaninhasImportadosDto?> BuscarPlanilhaPorIdAsync(string id, CancellationToken cancellationToken);
        Task<bool> RemoverPlanilhaPorIdAsync(string id, CancellationToken cancellationToken);
        Task<string> SalvarProdutoAsync(string planilhaId, SalvarProdutoDto dto, CancellationToken cancellationToken);
        Task SalvarPlanilhaImportadaAsync(PlaninhasImportadosDto dto, CancellationToken cancellationToken);
    }
}
