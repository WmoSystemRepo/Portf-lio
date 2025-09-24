using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface ITabelaPrecosPublicacoesService
    {
        Task<TabelaPrecoDto?> ObterConfiguracaoValidaAsync(int fabricanteId, CancellationToken ct);
    }
}
