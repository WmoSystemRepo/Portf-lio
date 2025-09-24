using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly IMongoCollection<ImportHistory> _collection;

        public ImportHistoryRepository(IOptions<MongoDbSettings> options)
        {
            var cfg = options.Value;

            var client = new MongoClient(cfg.ConnectionString);
            var database = client.GetDatabase(cfg.DatabaseName);

            _collection = database.GetCollection<ImportHistory>("ImportHistory");
        }

        public async Task InsertAsync(ImportHistory history, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _collection.InsertOneAsync(history);
        }

        public async Task<List<ImportHistory>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _collection.DeleteOneAsync(h => h.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
