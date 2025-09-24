namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara
{
    public class Deparas
    {
        public int Id { get; set; }
        public string? TipoExecucao { get; set; }
        public string? Integracao { get; set; }
        public string? CampoOrigem { get; set; }
        public string? ValorOrigem { get; set; }
        public string? CampoDestino { get; set; }
        public string? ValorDestino { get; set; }
    }
}
