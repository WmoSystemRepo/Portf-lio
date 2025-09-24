namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense
{
    public class IntegracaoBimmerHistoricoErrosInsercoes
    {
        public int Id { get; set; }
        public int IdResponse { get; set; }
        public long? Tentativas { get; set; }
        public DateTime? DataTentativa { get; set; }
        public string? MensagemErro { get; set; }
    }
}
