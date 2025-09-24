namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface ILogErroMigrationService
    {
        Task MigrarLogsAsync(CancellationToken cancellationToken);
    }
}