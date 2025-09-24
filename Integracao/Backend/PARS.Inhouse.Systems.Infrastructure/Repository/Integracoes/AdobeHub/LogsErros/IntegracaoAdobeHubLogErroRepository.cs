using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros
{
    /// <summary>
    /// Responsável por persistir e consultar logs de erro da integração Vexpenses → Bimmer no MongoDB.
    /// Essa classe é específica desta integração.
    /// </summary>
    public class IntegracaoAdobeHubLogErroRepository
    {
        private readonly Context _context;

        /// <summary>
        /// Construtor responsável por inicializar a collection do MongoDB com base nas configurações.
        /// </summary>
        /// <param name="settings">Configurações injetadas via IOptions contendo connection string, nome do banco e da collection.</param>
        public IntegracaoAdobeHubLogErroRepository(Context context)
        {
            try
            {
                _context = context;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Falha ao conectar com o MongoDB", ex);
            }
        }

        public async Task SaveAsync(IntegracaoAdobeHubLogsErros log, CancellationToken ct = default)
        {
            _context.Set<IntegracaoAdobeHubLogsErros>().Add(log);
            await _context.SaveChangesAsync(ct);
        }
    }
}
