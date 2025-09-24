using Microsoft.Extensions.Logging;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Templantes_Planinhas;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.VexpenssesBimer
{
    public class AdobeTemplantesExcelMongoService /*: ILogErroMigrationService*/
    {
        private readonly IntegracaoVexpensesBimmerLogErroRepository _mongoRepo;
        private readonly VexpensesBimmerLogErroRepository _sqlRepo;
        private readonly ILogger<LogErroMigrationService> _logger;

        public AdobeTemplantesExcelMongoService(
            IntegracaoVexpensesBimmerLogErroRepository mongoRepo,
            VexpensesBimmerLogErroRepository sqlRepo,
            ILogger<LogErroMigrationService> logger)
        {
            _mongoRepo = mongoRepo;
            _sqlRepo = sqlRepo;
            _logger = logger;
        }

        //public async Task MigrarLogsAsync(CancellationToken cancellationToken)
        //{
        //    var pendentes = await _logErroRepo.GetPendingMigrationAsync(DateTime.UtcNow.AddDays(-1));

        //    if (pendentes.Any())
        //    {
        //        foreach (var logMongo in pendentes)
        //        {
        //            var logSql = logMongo.ToSqlEntity();
        //            await _sqlRepo.SalvarAsync(logSql);
        //            await _logErroRepo.MarkAsMigratedAsync(logMongo.Id);

        //            _logger.LogInformation("Migrated log {LogId} to SQL.", logMongo.Id);
        //        }
        //    }
        //}
    }
}
