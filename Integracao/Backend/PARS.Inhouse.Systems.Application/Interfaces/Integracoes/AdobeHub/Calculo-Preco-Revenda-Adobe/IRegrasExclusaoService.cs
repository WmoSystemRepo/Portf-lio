using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public interface IRegrasExclusaoService
{
    Task<IEnumerable<RegraViewerDto>> ObterRegrasAsync(int fabricanteId, string segmento, CancellationToken ct);
}

