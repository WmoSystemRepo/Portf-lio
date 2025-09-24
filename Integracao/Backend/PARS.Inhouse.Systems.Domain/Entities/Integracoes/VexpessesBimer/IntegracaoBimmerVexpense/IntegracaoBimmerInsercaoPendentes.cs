namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense
{
    public class IntegracaoBimmerInsercaoPendentes
    {
        public int Id { get; set; }
        public int? IdResponse { get; set; }
        public int? UserId { get; set; }
        public string? Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string? Observacao { get; set; }
        public string? Response { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
