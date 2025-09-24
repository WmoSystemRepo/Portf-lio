namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class TabelaPrecoDto
    {
        public string MetodoMargemAdobe { get; set; } = "N";
        public decimal? MargemFixa { get; set; }
        public decimal CustoOperacional { get; set; }
        public decimal PIS { get; set; }
        public decimal COFINS { get; set; }
        public decimal ISS { get; set; }
        public decimal Marketing { get; set; }
        public decimal Outros { get; set; }
        public decimal? MargemMinima { get; set; }
        public decimal ProdNivel1 { get; set; }
        public decimal OutrosProd { get; set; }
    }
}
