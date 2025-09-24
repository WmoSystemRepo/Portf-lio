using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Response;

namespace PARS.Inhouse.Systems.Domain.Entities
{
    public class ImportHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? TemplateName { get; set; }

        public string? FileName { get; set; }

        public DateTime AttemptDate { get; set; }

        public bool Success { get; set; }

        public List<PendenciaImportacaoDto>? Pendencias { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
