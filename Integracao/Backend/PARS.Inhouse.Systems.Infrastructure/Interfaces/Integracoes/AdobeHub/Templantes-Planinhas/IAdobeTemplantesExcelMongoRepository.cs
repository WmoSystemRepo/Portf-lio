using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.SqlServe;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Templates.MongoDb;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub
{
    /// <summary>
    /// Interface responsável por interações com o MongoDB para templates e planilhas importadas no contexto da integração Adobe Hub.
    /// </summary>
    public interface IAdobeTemplantesExcelMongoRepository
    {
        #region 🔷 Templates

        /// <summary>
        /// Lista todos os templates de planilha armazenados no MongoDB.
        /// </summary>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns>Lista de objetos <see cref="AdobeTemplante"/> encontrados.</returns>
        Task<List<AdobeTemplante>> ListarTodosAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Cria ou atualiza (upsert) um template de planilha no MongoDB.
        /// Se um template com o mesmo nome já existir, ele será substituído.
        /// </summary>
        /// <param name="dto">DTO contendo as informações do template.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        Task NovoTemplante(TemplantesMongoDto dto, CancellationToken cancellationToken);

        /// <summary>
        /// Remove um template existente do MongoDB com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do template a ser removido.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns><c>true</c> se o template foi removido; <c>false</c> se não encontrado.</returns>
        Task<bool> RemoverTemplatePorIdAsync(string id, CancellationToken cancellationToken);

        #endregion

        #region 📥 Planilhas Importadas

        /// <summary>
        /// Lista todas as planilhas importadas já armazenadas no MongoDB.
        /// </summary>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns>Lista de objetos <see cref="PlanilhasImportadasMongo"/>.</returns>
        Task<List<PlanilhasImportadasMongo>> ListarPlanilhasImportadasAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Salva uma nova planilha importada no MongoDB.
        /// </summary>
        /// <param name="entidade">DTO representando a planilha com dados e metadados.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        Task SalvarPlanilhaImportadaAsync(PlanilhasImportadasMongo entidade, CancellationToken cancellationToken);

        /// <summary>
        /// Remove uma planilha existente do MongoDB com base no tipo informado.
        /// </summary>
        /// <param name="tipo">Tipo do template utilizado como critério de exclusão.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        Task RemoverPlanilhaImportadaPorTipo(string tipo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Salva o histórico de uma planilha (antes de sobrescrevê-la ou removê-la), no banco de dados relacional (SQL Server).
        /// </summary>
        /// <param name="integracaoAdobeHubHistoricoImportacaoExecel">Entidade contendo os dados da planilha em formato serializado.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        Task SalvarHistoricoAsync(
            IntegracaoAdobeHubHistoricoImportacaoExecel integracaoAdobeHubHistoricoImportacaoExecel,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Busca uma planilha importada no MongoDB com base no identificador único (ID).
        /// </summary>
        /// <param name="id">Identificador da planilha a ser localizada.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns>
        /// Objeto <see cref="PlanilhasImportadasMongo"/> correspondente ao ID fornecido, ou <c>null</c> se não encontrado.
        /// </returns>
        Task<PlanilhasImportadasMongo?> BuscarPlanilhaPorIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Retorna o template correspondente ao tipo informado.
        /// </summary>
        /// <param name="tipo">Tipo do template desejado.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns>
        /// Objeto <see cref="AdobeTemplante"/> correspondente ao tipo solicitado, ou <c>null</c> se não existir.
        /// </returns>
        Task<AdobeTemplante?> ObterTemplatePorTipo(string tipo, CancellationToken cancellationToken);

        /// <summary>
        /// Remove um template existente do MongoDB com base no nome fornecido.
        /// </summary>
        /// <param name="nome">Nome do template a ser removido.</param>
        /// <param name="cancellationToken">Token para cancelamento assíncrono da operação.</param>
        /// <returns><c>true</c> se o template foi removido; <c>false</c> se não encontrado.</returns>
        Task<bool> RemoverPlanilhaPorIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Adiciona o <paramref name="produto"/> ao final do array "Dados" da planilha indicada.
        /// </summary>
        Task AdicionarProdutoNoFinalAsync(string planilhaId, IDictionary<string, object?> produto, CancellationToken cancellationToken);

        #endregion
    }
}
