using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu
{
    public class AnyPointStoreMenuIntegracao
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public int IntegracaoId { get; set; }

        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public bool Ativo { get; set; } = true;


        public AnyPointStoreMenu Menu { get; set; } = null!;
        public AnyPointStoreGestaoIntegracao Integracao { get; set; } = null!;
    }
}
