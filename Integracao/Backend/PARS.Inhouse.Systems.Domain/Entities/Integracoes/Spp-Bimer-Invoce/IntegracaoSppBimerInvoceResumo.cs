public class IntegracaoSppBimerInvoceResumo
{
    public int CdEmpresa { get; set; }
    public string NumeroDocumento { get; set; }
    public string NumeroDaNFSe { get; set; }

    public DateTime DataDeVencimento { get; set; }
    public decimal ValorPagamentoTotal { get; set; }

    public IntegracaoSppBimerInvoce Invoce { get; set; }
}
