namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub;

public sealed class ConstantesPedido
{
    public int Id { get; set; }
    public int FabricanteId { get; set; }          
    public string Segmento { get; set; } = string.Empty; 
    public string MetodoMargemAdobe { get; set; } = "N"; 
    public decimal? MargemFixa { get; set; }             
}
