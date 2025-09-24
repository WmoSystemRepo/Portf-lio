using PARS.Inhouse.Systems.Domain.Entities;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub
{
    public interface IImportHistoryRepository
    {
        Task InsertAsync(ImportHistory history, CancellationToken cancellationToken);
        Task<List<ImportHistory>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
    }
}