using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.ExclussãoPendecias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb
{
    public class IntegracaoAdobeHubPlanilhaExclusaoLogMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime DataHora { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string Justificativa { get; set; } = string.Empty;

        public List<RegistroExcluidoPlanilhaMongo> RegistrosExcluidos { get; set; } = new();

        public bool MigradoParaSql { get; set; }

        public string Endpoint { get; set; } = string.Empty;
    }
}
