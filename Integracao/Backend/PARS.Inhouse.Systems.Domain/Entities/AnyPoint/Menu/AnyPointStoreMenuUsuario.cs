namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu
{
    public class AnyPointStoreMenuUsuario
    {
        public int Id { get; set; }

        public int MenuId { get; set; }
        public string UsuarioId { get; set; }

        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public bool Ativo { get; set; } = true;

        public AnyPointStoreMenu Menu { get; set; } = null!;
    }
}
