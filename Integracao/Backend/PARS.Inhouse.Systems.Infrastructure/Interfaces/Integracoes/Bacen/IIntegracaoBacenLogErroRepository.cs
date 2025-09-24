using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Bacen
{
    public interface IIntegracaoBacenLogErroRepository
    {
        Task SaveAsync(IntegracaoBacenLogErros log, CancellationToken ct = default);
    }
}