namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario
{
    public class AnyPointStoreUsuarioRegra
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public string RegraId { get; set; }

        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
