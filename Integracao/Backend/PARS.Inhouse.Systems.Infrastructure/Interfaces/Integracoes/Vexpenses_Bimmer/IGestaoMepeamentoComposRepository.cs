using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento;
using System.Linq.Expressions;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IGestaoMepeamentoComposRepository
    {
        Task<IEnumerable<AnyPointDeparas>> ListarMapeamentoCamposAsync(CancellationToken cancellationToken = default);
        Task<AnyPointDeparas?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<AnyPointDeparas?> GetDeparaAsync(Expression<Func<AnyPointDeparas, bool>> expression, CancellationToken cancellationToken = default);
        Task NovoRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken = default);
        Task EditarRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken = default);
        Task ExcluirRegistroAsync(int id, CancellationToken cancellationToken = default);

        #region Interfaces de Repositório de Mapeamento com Referência à Integracão

        Task MapeamentoIntegracaoRepositorioRegistrarAsync(List<MapeamentoIntegracaoDto> menus, CancellationToken cancellationToken);
        Task<List<AnyPointStoreMapeamentoIntegracao>?> MapeamentoIntegracaoRepositorioBuscarPorIdMapeamentoAsync(int idMenu, CancellationToken cancellationToken);
        Task<AnyPointStoreMapeamentoIntegracao> MapeamentoIntegracaoRepositorioBuscarPorIdReferenciaAsync(int mapeamentoId, int integracaoId, CancellationToken cancellationToken);
        Task MapeamentoIntegracaoRepositorioDeletarAsync(AnyPointStoreMapeamentoIntegracao menuIntegracao, CancellationToken cancellationToken);

        #endregion
    }
}