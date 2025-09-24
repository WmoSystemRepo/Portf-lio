using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface IPrecoRevendaService
    {
        Task<decimal> CalcularPrecoRevendaAsync(
            decimal fob,
            string nivel,
            IndicePrecoRevendaDto idx,
            CancellationToken cancellationToken = default);

        Task AplicarPrecoRevendaAsync(
            int fabricanteId,
            string segmento,
            IList<Dictionary<string, object?>> linhas,
            IndicePrecoRevendaDto idx,
            CancellationToken cancellationToken = default);
    }
}