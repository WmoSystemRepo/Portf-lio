using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public class TabelaPrecosPublicacoesService : ITabelaPrecosPublicacoesService
{
    private readonly ITabelaPrecosPublicacoesRepository _repository;

    public TabelaPrecosPublicacoesService(ITabelaPrecosPublicacoesRepository repository)
    {
        _repository = repository;
    }

    public async Task<TabelaPrecoDto?> ObterConfiguracaoValidaAsync(int fabricanteId, CancellationToken ct)
    {
        return await _repository.ObterConfiguracaoValidaAsync(fabricanteId, ct);
    }
}
