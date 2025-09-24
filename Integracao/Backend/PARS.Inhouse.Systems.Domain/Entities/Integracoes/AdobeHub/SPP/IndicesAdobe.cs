namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub;

public sealed class IndicesAdobe
{
    public required decimal PIS { get; init; }              
    public required decimal COFINS { get; init; }            
    public required decimal ISS { get; init; }               
    public required decimal CustoOperacional { get; init; }  
    public required decimal ProdNivel1 { get; init; }        
    public required decimal OutrosProd { get; init; }        
    public decimal? MargemMinima { get; init; }              
}
