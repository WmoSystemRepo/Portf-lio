using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface IConstantesPedidoRepository
    {
        Task<ConstantesPedido> ObterAsync(int fabricanteId, string segmento, CancellationToken ct);
    }
}
