namespace PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Regras
{
    using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Regras;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRegrasService
    {
        Task<List<RegraDto>> ObterTodasRegrasAsync(CancellationToken cancellationToken);
        Task<RegraDto?> ObterRegraPorIdAsync(string id, CancellationToken cancellationToken);
        Task<bool> CriarRegraAsync(RegraDto dto, CancellationToken cancellationToken);
        Task<bool> AtualizarRegraAsync(string id, RegraDto dto, CancellationToken cancellationToken);
        Task<bool> ExcluirRegraAsync(string nome, CancellationToken cancellationToken);
    }
}
