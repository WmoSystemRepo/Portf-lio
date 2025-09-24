using System.ComponentModel.DataAnnotations.Schema;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara
{
    public class AnyPointDeparas
    {
        public int Id { get; set; }

        [ForeignKey("IntegracaoMapeamentoDeCampos")]
        public int? IdCampos { get; set; }
        public string? TipoExecucao { get; set; }
        public string? Integracao { get; set; }
        public string? NomeMapeamentoOrigem { get; set; }
        public string? CampoOrigem { get; set; }
        public string? CodigoSistemaOrigem { get; set; }
        public string? ValorOrigem { get; set; }
        public string? NomeMapeamentoDestino { get; set; }
        public string? CampoDestino { get; set; }
        public string? CodigoSistemaDestino { get; set; }
        public string? ValorDestino { get; set; }
    }
}
