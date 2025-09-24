using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Templates.MongoDb;

namespace PARS.Inhouse.Systems.Infrastructure.Data.DbContext
{
    public class MongoAdobeTemplateDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbSettings _settings;

        public MongoAdobeTemplateDbContext(IOptions<MongoDbSettings> options)
        {
            _settings = options.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoCollection<AdobeTemplante> TemplatePlanilhas =>
            _database.GetCollection<AdobeTemplante>(_settings.CollectionName);
    }
}
