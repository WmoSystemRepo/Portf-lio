using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb
{
    /// <summary>
    /// Representa uma planilha importada salva no MongoDB.
    /// </summary>
    public class PlanilhasImportadasMongo
    {
        /// <summary>
        /// Identificador único do documento no MongoDB.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Nome do arquivo Excel que foi importado.
        /// </summary>
        [BsonElement("nomeArquivo")]
        public string NomeArquivo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora do upload da planilha.
        /// </summary>
        [BsonElement("dataUpload")]
        public string? DataUpload { get; set; }

        public string? Usuario { get; set; }

        /// <summary>
        /// Matriz contendo os dados da planilha (linhas e colunas).
        /// </summary>
        [BsonElement("dados")]
        public List<Dictionary<string, string>>? Dados { get; set; }

        public int Versao { get; set; }

        public TemplantesMongoDto? Template { get; set; }
    }
}
