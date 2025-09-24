using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.ExclussãoPendecias;

public class IntegracaoVexpensesBimmerExclusaoLogMongo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime DataHora { get; set; }

    public string Usuario { get; set; } = string.Empty;

    public string Justificativa { get; set; } = string.Empty;

    public List<RegistroExcluidoMongo> RegistrosExcluidos { get; set; } = new();

    public bool MigradoParaSql { get; set; }

    public string Endpoint { get; set; } = string.Empty;
}
