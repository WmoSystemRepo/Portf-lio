namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public sealed class CalcularPrecoResponseDto
{
    public required int Total { get; init; }
    public required List<LinhaCalculada> Linhas { get; init; }

    public sealed class LinhaCalculada
    {
        public string PartNumber { get; init; } = string.Empty;
        public decimal FOB { get; init; }
        public decimal PrecoRevendaUS { get; init; }
        public string? Observacao { get; init; }
    }
}
