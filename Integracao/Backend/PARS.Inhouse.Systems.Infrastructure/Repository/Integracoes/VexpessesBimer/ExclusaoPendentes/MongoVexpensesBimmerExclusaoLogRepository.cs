
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ExclusaoPendentes
{
    public class MongoVexpensesBimmerExclusaoLogRepository
    {
        private readonly IMongoCollection<IntegracaoVexpensesBimmerExclusaoLogMongo> _collection;

        public MongoVexpensesBimmerExclusaoLogRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _collection = database.GetCollection<IntegracaoVexpensesBimmerExclusaoLogMongo>("exclusao_vexpesses_bimer_log");
        }

        public async Task SaveAsync(IntegracaoVexpensesBimmerExclusaoLogMongo log, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(log, null, cancellationToken);
        }

        public async Task<List<IntegracaoVexpensesBimmerExclusaoLogMongo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<long> CountAsync(FilterDefinition<IntegracaoVexpensesBimmerExclusaoLogMongo> filter, CancellationToken cancellationToken = default)
        {
            return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task<List<IntegracaoVexpensesBimmerExclusaoLogMongo>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
