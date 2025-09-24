using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.SqlServe;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Templates.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;
using System.Text.Json;


namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub
{
    /// <summary>
    /// Repositório responsável por operações no MongoDB relacionadas a templates e planilhas importadas do Adobe Hub.
    /// </summary>
    public class AdobeTemplantesExcelMongoDbRepository : IAdobeTemplantesExcelMongoRepository
    {
        private readonly IMongoCollection<AdobeTemplante> _collection;
        private readonly IMongoCollection<PlanilhasImportadasMongo> _planilhaCollection;
        private readonly Context _context;

        public AdobeTemplantesExcelMongoDbRepository(IMongoDatabase mongoDatabase, Context context)
        {
            _collection = mongoDatabase.GetCollection<AdobeTemplante>("adobe_templantes_excel");
            _planilhaCollection = mongoDatabase.GetCollection<PlanilhasImportadasMongo>("adobe_importacao_panilhas");
            _context = context;
        }

        public async Task<List<AdobeTemplante>> ListarTodosAsync(CancellationToken cancellationToken)
        {
            try
            {
                var cursor = await _collection.FindAsync(FilterDefinition<AdobeTemplante>.Empty, cancellationToken: cancellationToken);
                return await cursor.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar templates no MongoDB.", ex);
            }
        }

        public async Task NovoTemplante(TemplantesMongoDto dto, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var filtro = Builders<AdobeTemplante>.Filter.Eq(nameof(AdobeTemplante.Nome), dto.Nome);

            var entidade = new AdobeTemplante
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Nome = dto.Nome,
                QtdColunas = dto.QtdColunas,
                Colunas = dto.Colunas,
                LinhaCabecalho = dto.LinhaCabecalho,
                ColunaInicial = dto.ColunaInicial,
                ArquivoBase = dto.ArquivoBase,
                ObservacaoDescricao = dto.ObservacaoDescricao,
                ColunasObrigatorias = dto.ColunasObrigatorias,
                DataCriacao = dto.DataCriacao ?? DateTime.UtcNow.ToString("o"),
                DataEdicao = dto.DataEdicao ?? DateTime.UtcNow.ToString("o"),
                TipoTemplante = dto.TipoTemplate
            };

            await _collection.ReplaceOneAsync(
                filtro,
                entidade,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken
            );
        }


        /// <inheritdoc/>
        public async Task<bool> RemoverTemplatePorIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID do template não pode ser vazio.", nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var filtro = Builders<AdobeTemplante>.Filter.Eq(t => t.Id, id);
            var resultado = await _collection.DeleteOneAsync(filtro, cancellationToken);

            return resultado.DeletedCount > 0;
        }

        public async Task<List<PlanilhasImportadasMongo>> ListarPlanilhasImportadasAsync(CancellationToken cancellationToken)
        {
            var cursor = await _planilhaCollection.FindAsync(FilterDefinition<PlanilhasImportadasMongo>.Empty, cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        public async Task SalvarPlanilhaImportadaAsync(PlanilhasImportadasMongo entidade, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _planilhaCollection.InsertOneAsync(entidade, cancellationToken: cancellationToken);
        }


        public async Task RemoverPlanilhaImportadaPorTipo(string tipo, CancellationToken cancellationToken)
        {
            var filtro = Builders<PlanilhasImportadasMongo>.Filter.Eq(nameof(PlanilhasImportadasMongo.Template.TipoTemplate), tipo);
            await _planilhaCollection.DeleteOneAsync(filtro, cancellationToken);
        }

        public async Task SalvarHistoricoAsync(IntegracaoAdobeHubHistoricoImportacaoExecel historico, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(historico.ExcelJson))
            {
                throw new ArgumentException("O campo ExcelJson não pode estar vazio.");
            }

            var jsonCompleto = historico.ExcelJson;
            historico.ExcelJson = "";

            _context.IntegracaoAdobeHubHistoricoImportacaoExecel.Add(historico);
            await _context.SaveChangesAsync(cancellationToken);

            const string sql = @"
                UPDATE IntegracaoAdobeHubHistoricoImportacaoExecel
                SET ExcelJson = @json
                WHERE Id = @id";

            await _context.Database.ExecuteSqlRawAsync(sql, new[]
            {
                new SqlParameter("@json", jsonCompleto ?? (object)DBNull.Value),
                new SqlParameter("@id", historico.Id)
            }, cancellationToken);
        }

        public async Task<PlanilhasImportadasMongo?> BuscarPlanilhaPorIdAsync(string id, CancellationToken cancellationToken)
        {
            var filtro = Builders<PlanilhasImportadasMongo>.Filter.Eq(nameof(PlanilhasImportadasMongo.Id), id);
            return await _planilhaCollection.Find(filtro).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AdobeTemplante?> ObterTemplatePorTipo(string tipo, CancellationToken cancellationToken)
        {
            var filtro = Builders<AdobeTemplante>.Filter.Eq(nameof(AdobeTemplante.TipoTemplante), tipo);
            return await _collection.Find(filtro).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> RemoverPlanilhaPorIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID da Planilha não pode ser vazio.", nameof(id));

            var filtro = Builders<PlanilhasImportadasMongo>.Filter.Eq(nameof(PlanilhasImportadasMongo.Id), id);
            var resultado = await _planilhaCollection.DeleteOneAsync(filtro, cancellationToken);

            return resultado.DeletedCount > 0;
        }

        public async Task AdicionarProdutoNoFinalAsync(string planilhaId, IDictionary<string, object?> produto, CancellationToken cancellationToken)
        {
            // 🔒 Validações
            if (string.IsNullOrWhiteSpace(planilhaId))
                throw new ArgumentException("ID da planilha inválido.", nameof(planilhaId));

            if (produto is null || produto.Count == 0)
                throw new ArgumentException("Produto inválido ou vazio.", nameof(produto));

            // 🔍 Busca a planilha existente
            var filtro = Builders<PlanilhasImportadasMongo>.Filter.Eq(p => p.Id, planilhaId);
            var planilha = await _planilhaCollection.Find(filtro).FirstOrDefaultAsync(cancellationToken);

            if (planilha == null)
                throw new InvalidOperationException($"Planilha com ID '{planilhaId}' não encontrada.");

            // 📦 Copia os dados existentes convertendo para BsonDocument
            var dadosAtuais = new List<BsonDocument>();

            if (planilha.Dados != null)
            {
                foreach (var item in planilha.Dados)
                {
                    var doc = new BsonDocument(
                        item.Select(kv =>
                            new BsonElement(kv.Key, kv.Value is null ? BsonNull.Value : BsonValue.Create(kv.Value)))
                    );
                    dadosAtuais.Add(doc);
                }
            }

            // ➕ Converte e adiciona o novo produto no final
            var novoProduto = new BsonDocument(
                produto.Select(kv =>
                {
                    if (kv.Value == null)
                        return new BsonElement(kv.Key, BsonNull.Value);

                    if (kv.Value is JsonElement jsonElement)
                    {
                        // Faz parse do JsonElement para string (ou outro tipo conforme necessidade)
                        switch (jsonElement.ValueKind)
                        {
                            case JsonValueKind.String:
                                return new BsonElement(kv.Key, jsonElement.GetString());
                            case JsonValueKind.Number:
                                if (jsonElement.TryGetInt32(out int intValue))
                                    return new BsonElement(kv.Key, intValue);
                                if (jsonElement.TryGetDecimal(out decimal decValue))
                                    return new BsonElement(kv.Key, decValue);
                                return new BsonElement(kv.Key, jsonElement.GetRawText());

                            case JsonValueKind.True:
                            case JsonValueKind.False:
                                return new BsonElement(kv.Key, jsonElement.GetBoolean());

                            case JsonValueKind.Null:
                                return new BsonElement(kv.Key, BsonNull.Value);

                            default:
                                return new BsonElement(kv.Key, jsonElement.ToString());
                        }
                    }

                    return new BsonElement(kv.Key, BsonValue.Create(kv.Value));
                })
            );

            dadosAtuais.Add(novoProduto);

            // 🔁 Atualiza o campo "Dados" da planilha com os dados atualizados
            var update = Builders<PlanilhasImportadasMongo>.Update.Set("Dados", dadosAtuais);

            await _planilhaCollection.UpdateOneAsync(
                filter: filtro,
                update: update,
                cancellationToken: cancellationToken);
        }
    }
}
