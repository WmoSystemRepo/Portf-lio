namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce
{
    /// <summary>
    /// DTO de monitoramento da integração de invoices entre SPP e Bimer.
    /// Representa as informações necessárias para exibir e filtrar dados na tela de acompanhamento.
    /// </summary>
    public class MonitoramentoSppBimerInvoceDto
    {
        public int Id { get; set; }
        public string NumeroAlterData { get; set; } = string.Empty;
        public string? PedidoSpp { get; set; }
        public string Fabricante { get; set; } = string.Empty;
        public string? FabricanteId { get; set; }
        public string Estoque { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public decimal ValorInvoice { get; set; }
        public string StatusIntegracao { get; set; } = "N";
        public string ObservacaoErro { get; set; } = string.Empty;
    }

    public class ReprocessarBimerRequestDto
    {
        public string Pedido { get; set; } = string.Empty;
        public string Estoque { get; set; } = string.Empty;
        public string? Fabricante { get; set; }
    }
}