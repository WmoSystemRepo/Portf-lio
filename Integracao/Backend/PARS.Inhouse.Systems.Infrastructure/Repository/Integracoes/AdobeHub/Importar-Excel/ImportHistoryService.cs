using PARS.Inhouse.Systems.Domain.Entities;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub
{
    public class ImportHistoryService
    {
        private readonly IImportHistoryRepository _importHistoryRepository;

        public ImportHistoryService(IImportHistoryRepository importHistoryRepository)
        {
            _importHistoryRepository = importHistoryRepository;
        }

        /// <summary>
        /// Salva um histórico de importação no MongoDB.
        /// </summary>
        public async Task SaveImportAttemptAsync(ImportHistory history, CancellationToken cancellationToken)
        {
            if (history == null)
            {
                throw new ArgumentNullException(nameof(history), "O histórico não pode ser nulo.");
            }

            history.Id = Guid.NewGuid().ToString();
            history.Timestamp = DateTime.UtcNow;

            await _importHistoryRepository.InsertAsync(history, cancellationToken);
        }

        /// <summary>
        /// Lista todos os históricos de importação.
        /// </summary>
        public async Task<List<ImportHistory>> GetAllImportHistoryAsync(CancellationToken cancellationToken)
        {
            return await _importHistoryRepository.GetAllAsync(cancellationToken);
        }

        /// <summary>
        /// Remove um histórico específico pelo ID.
        /// </summary>
        public async Task<bool> DeleteImportHistoryByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("O ID não pode ser vazio.", nameof(id));
            }

            return await _importHistoryRepository.DeleteByIdAsync(id, cancellationToken);
        }
    }
}
