using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros
{
    public class MongoAdobeHubLogExclusaoPlanilhaRepository
    {
        private readonly IMongoCollection<IntegracaoAdobeHubPlanilhaExclusaoLogMongo> _collection;

        public MongoAdobeHubLogExclusaoPlanilhaRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _collection = database.GetCollection<IntegracaoAdobeHubPlanilhaExclusaoLogMongo>("exclusao_adobe_hub_planilha_log");
        }

        public async Task SaveAsync(IntegracaoAdobeHubPlanilhaExclusaoLogMongo log, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(log, null, cancellationToken);
        }

        public async Task<List<IntegracaoAdobeHubPlanilhaExclusaoLogMongo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<long> CountAsync(FilterDefinition<IntegracaoAdobeHubPlanilhaExclusaoLogMongo> filter, CancellationToken cancellationToken = default)
        {
            return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task<List<IntegracaoAdobeHubPlanilhaExclusaoLogMongo>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
