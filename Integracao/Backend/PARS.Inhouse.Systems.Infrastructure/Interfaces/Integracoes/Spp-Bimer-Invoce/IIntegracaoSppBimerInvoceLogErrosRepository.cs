using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce
{
    public interface IIntegracaoSppBimerInvoceLogErrosRepository
    {
        Task SaveAsync(IntegracaoSppBimerInvoceLogErros log, CancellationToken ct = default);
    }
}
