namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao
{
    public class AnyPointStoreGestaoIntegracao
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string ProjetoOrigem { get; set; } = string.Empty;
        public string ProjetoDestino { get; set; } = string.Empty;
        public string DataCriacao { get; set; } = string.Empty;
        public string DataEdicao { get; set; } = string.Empty;
    }
}
