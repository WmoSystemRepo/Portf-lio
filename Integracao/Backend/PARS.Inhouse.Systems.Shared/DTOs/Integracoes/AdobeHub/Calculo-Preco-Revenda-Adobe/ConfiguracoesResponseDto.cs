namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public sealed class ConfiguracoesResponseDto
{
    public char? MetodoMargemAdobe { get; init; }
    public decimal? MargemFixa { get; init; }
    public decimal? PIS { get; init; }
    public decimal? COFINS { get; init; }
    public decimal? ISS { get; init; }
    public decimal? CustoOperacional { get; init; }
    public decimal? ProdNivel1 { get; init; }
    public decimal? OutrosProd { get; init; }
    public decimal? MargemMinima { get; init; }
}
