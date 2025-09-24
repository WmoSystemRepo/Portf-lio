namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara
{
    public class AnyPointStoreMapeamentoIntegracao
    {
        public int Id { get; set; }

        public int MapeamentoId { get; set; }
        public int IntegracaoId { get; set; }

        public string? DataCriacao { get; set; }
        public string? DataEdicao { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
