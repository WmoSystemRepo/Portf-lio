namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense
{
    public class IntegracaoBimmerInsertOK
    {
        public int Id { get; set; }
        public int IdResponse { get; set; }
        public string? IdentificadorBimmer { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string? Observacao { get; set; }
        public string? Response { get; set; }
    }
}
