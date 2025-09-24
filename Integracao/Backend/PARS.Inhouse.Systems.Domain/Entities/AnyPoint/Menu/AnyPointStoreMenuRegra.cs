namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu
{
    public class AnyPointStoreMenuRegra
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string RegraId { get; set; }

        public bool Ativo { get; set; } = true;
        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public AnyPointStoreMenu? Menu { get; set; }
    }
}
