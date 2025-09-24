namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.ExclussãoPendecias
{
    public class RegistroExcluidoMongo
    {
        public int IdResponse { get; set; }

        public string? Descricao { get; set; }
        public int UserId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public string? Observacao { get; set; }
        public string? Response { get; set; }
    }
}
