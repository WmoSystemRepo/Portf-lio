namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.SPP
{
    public sealed class TabelaPrecosPublicacoes
    {
        public int Id { get; set; }

        public int FabricanteId { get; set; }
        public string Segmento { get; set; } = string.Empty;

        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public bool Ativo { get; set; }
    }

}
