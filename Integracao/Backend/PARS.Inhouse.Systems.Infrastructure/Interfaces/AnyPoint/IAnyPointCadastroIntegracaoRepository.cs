using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint
{
    public interface IAnyPointCadastroIntegracaoRepository
    {
        Task<IEnumerable<AnyPointStoreGestaoIntegracao>> GetAllAsync(CancellationToken cancellationToken);
        Task<AnyPointStoreGestaoIntegracao?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(AnyPointStoreGestaoIntegracao integracao, CancellationToken cancellationToken);
        Task UpdateAsync(AnyPointStoreGestaoIntegracao integracao, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}