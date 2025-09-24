namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen
{
    public class IntegracaoBacenCotacaoMoeda
    {
        public int Id { get; set; }
        public string CodigoMoeda { get; set; } = string.Empty;
        public DateTime DataHoraCotacao { get; set; }
        public decimal CotacaoCompra { get; set; }
        public decimal CotacaoVenda { get; set; }
        public string TipoBoletim { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public DateTime DataHoraIntegracao { get; set; }
    }
}
