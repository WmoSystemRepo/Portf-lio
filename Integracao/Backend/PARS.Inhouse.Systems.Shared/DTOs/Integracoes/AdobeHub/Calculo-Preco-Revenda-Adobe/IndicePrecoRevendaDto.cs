namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

public class IndicePrecoRevendaDto
{
    public decimal CustoOperacional { get; set; }
    public decimal PIS { get; set; }
    public decimal COFINS { get; set; }
    public decimal ICMS { get; set; }
    public decimal ISS { get; set; }
    public decimal Marketing { get; set; }
    public decimal Outros { get; set; }

    public decimal ProdNivel1 { get; set; }
    public decimal OutrosProd { get; set; }
}
