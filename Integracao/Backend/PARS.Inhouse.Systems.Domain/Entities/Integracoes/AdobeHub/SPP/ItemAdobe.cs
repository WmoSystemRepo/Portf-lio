namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub;

public sealed class ItemAdobe
{
    public string PartNumber { get; init; } = string.Empty;
    public string? LevelDetail { get; init; }
    public decimal FOB { get; init; }        
}
