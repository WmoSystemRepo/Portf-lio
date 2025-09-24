using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.CadastroIntegracao;

namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint
{
    public interface IAnyPointCadastroIntegracaoService
    {
        Task<IEnumerable<GestaoIntegracoesDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<GestaoIntegracoesDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task AddAsync(GestaoIntegracoesDto dto, CancellationToken cancellationToken);
        Task UpdateAsync(GestaoIntegracoesDto dto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
