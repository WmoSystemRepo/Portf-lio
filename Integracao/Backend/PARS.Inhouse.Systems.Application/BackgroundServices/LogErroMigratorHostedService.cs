// File: LogErroMigratorHostedService.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.Vexpenses_Bimmer;

namespace PARS.Inhouse.Systems.Application.BackgroundServices
{
    public class LogErroMigratorHostedService : BackgroundService
    {
        private readonly ILogger<LogErroMigratorHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(5);

        public LogErroMigratorHostedService(
            ILogger<LogErroMigratorHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🔄 LogErroMigratorHostedService iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                //var migrationService = scope.ServiceProvider.GetRequiredService<ILogErroMigrationService>();

                try
                {
                    _logger.LogInformation("🚧 Iniciando execução de migração de logs...");
                    //await migrationService.MigrarLogsAsync(stoppingToken);
                    _logger.LogInformation("✅ Migração de logs concluída com sucesso.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erro durante a migração dos logs.");
                }

                try
                {
                    _logger.LogInformation("⏳ Aguardando {Intervalo} até a próxima execução.", _intervalo);
                    await Task.Delay(_intervalo, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("🛑 Execução cancelada durante o tempo de espera. Finalizando serviço.");
                    break;
                }
            }

            _logger.LogInformation("🏁 LogErroMigratorHostedService finalizado.");
        }

    }
}
