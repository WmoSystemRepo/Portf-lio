using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Response
{
    public class ImportHistoryDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? TemplateName { get; set; }

        public string? FileName { get; set; }

        public DateTime AttemptDate { get; set; }

        public bool Success { get; set; }

        public List<PendenciaImportacaoDto>? Pendencias { get; set; }
    }
}
