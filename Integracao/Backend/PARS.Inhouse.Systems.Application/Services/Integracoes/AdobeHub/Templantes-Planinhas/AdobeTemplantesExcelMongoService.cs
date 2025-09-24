using Microsoft.Extensions.Logging;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Templantes_Planinhas
{
    public class LogErroMigrationService/* : ILogErroMigrationService*/
    {
        private readonly IntegracaoVexpensesBimmerLogErroRepository _logErroRepo;
        private readonly VexpensesBimmerLogErroRepository _sqlRepo;
        private readonly ILogger<LogErroMigrationService> _logger;

        public LogErroMigrationService(
            IntegracaoVexpensesBimmerLogErroRepository mongoRepo,
            VexpensesBimmerLogErroRepository sqlRepo,
            ILogger<LogErroMigrationService> logger)
        {
            _logErroRepo = mongoRepo;
            _sqlRepo = sqlRepo;
            _logger = logger;
        }

        //public async task migrarlogsasync(cancellationtoken cancellationtoken)
        //{
        //    var pendentes = await _logerrorepo.getpendingmigrationasync(datetime.utcnow.adddays(-1));

        //    if (pendentes.any())
        //    {
        //        foreach (var logmongo in pendentes)
        //        {
        //            var logsql = logmongo.tosqlentity();
        //            await _sqlrepo.salvarasync(logsql);
        //            await _logerrorepo.markasmigratedasync(logmongo.id);

        //            _logger.loginformation("migrated log {logid} to sql.", logmongo.id);
        //        }
        //    }
        //}
    }
}
