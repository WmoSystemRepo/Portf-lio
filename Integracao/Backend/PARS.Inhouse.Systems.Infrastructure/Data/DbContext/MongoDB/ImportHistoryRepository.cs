using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities;

namespace PARS.Inhouse.Systems.Infrastructure.Data.DbContext.MongoDB
{
    public class ImportHistoryRepository
    {
        private readonly IMongoCollection<ImportHistory> _collection;

        public ImportHistoryRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<ImportHistory>("ImportHistoryDto");
        }

        public async Task SaveAsync(ImportHistory history)
        {
            await _collection.InsertOneAsync(history);
        }

        public async Task<List<ImportHistory>> GetAllAsync()
        {
            return await _collection.Find(_ => true)
                                    .SortByDescending(h => h.AttemptDate)
                                    .ToListAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _collection.DeleteOneAsync(h => h.Id == id);
        }
    }
}