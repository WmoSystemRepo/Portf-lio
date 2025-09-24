using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface IConfiguracoesService
    {
        Task<ConfiguracoesResponseDto> Handle(int fabricanteId, string segmento, CancellationToken ct);
    }

}
