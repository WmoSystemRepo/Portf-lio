using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface IRegrasExclusaoRepository
    {
        Task<IReadOnlyList<RegraViewerDto>> ObterPorFabricanteSegmentoAsync(int fabricanteId, string segmento, CancellationToken ct);
    }
}
