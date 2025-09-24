namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public sealed class CalcularPrecoRequestDto
{
    public required int FabricanteId { get; init; }
    public required string Segmento { get; init; }            
    public decimal? MargemBrutaFormulario { get; init; }      
    public List<CalcularItemLinha> Linhas { get; init; } = new();

    public sealed class CalcularItemLinha
    {
        public string PartNumber { get; init; } = string.Empty;
        public string? LevelDetail { get; init; }
        public decimal FOB { get; init; }
    }
}
