using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IGestaoMepeamentoComposService
    {
        Task<IEnumerable<AnyPointDeparas>> ListarMapeamentoCamposAsync(CancellationToken cancellationToken);

        Task<AnyPointDeparas?> ObterPorIdAsync(int id, CancellationToken cancellationToken);

        Task NovoRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken);

        Task EditarRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken);

        Task ExcluirRegistroAsync(int id, CancellationToken cancellationToken);


        Task MapeamentoIntegracaoServicoRegistrarAsync(List<MapeamentoIntegracaoDto> integracoes, CancellationToken cancellationToken);
        Task<AnyPointStoreMapeamentoIntegracao> MapeamentoIntegracaoServicoBuscaPorIdReferenciaAsync(int mapeamentoid, int integracaoId, CancellationToken cancellationToken);
        Task<List<AnyPointStoreMapeamentoIntegracao>> MapeamentoIntegracaoServicoBuscarPorIdMapeamentoAsync(int idMapeamento, CancellationToken cancellationToken);
        Task MapeamentoIntegracaoServicoDeletarAsync(AnyPointStoreMapeamentoIntegracao mapeamentoIntegracao, CancellationToken cancellationToken);
    }
}
