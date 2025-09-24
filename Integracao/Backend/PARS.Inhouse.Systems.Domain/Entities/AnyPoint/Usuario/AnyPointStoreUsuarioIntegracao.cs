using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario
{
    public class AnyPointStoreUsuarioIntegracao
    {
        public int Id { get; set; }

        public string UsuarioId { get; set; }
        public int IntegracaoId { get; set; }

        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public bool Ativo { get; set; } = true;

        public AnyPointStoreGestaoIntegracao Integracao { get; set; } = null!;
    }
}
