using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public class RegrasExclusaoService : IRegrasExclusaoService
{
    private readonly IRegrasExclusaoRepository _regrasReposytory;

    public RegrasExclusaoService(IRegrasExclusaoRepository regrasReposytory)
    {
        _regrasReposytory = regrasReposytory;
    }

    public async Task<IEnumerable<RegraViewerDto>> ObterRegrasAsync(int fabricanteId, string segmento, CancellationToken ct)
    {
        return await _regrasReposytory.ObterPorFabricanteSegmentoAsync(fabricanteId, segmento, ct);
    }
}
