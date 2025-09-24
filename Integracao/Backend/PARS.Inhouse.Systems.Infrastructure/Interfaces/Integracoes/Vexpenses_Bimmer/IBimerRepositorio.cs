using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IBimerRepositorio
    {
        Task RegistrarTitulosPendentesAsync(IntegracaoBimmerInsercaoPendentes pendente, CancellationToken cancellationToken = default);
        Task RemovePendenteByTitleIdAsync(int titleId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IntegracaoBimmerInsercaoPendentes>> GetPendenciasAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    }
}