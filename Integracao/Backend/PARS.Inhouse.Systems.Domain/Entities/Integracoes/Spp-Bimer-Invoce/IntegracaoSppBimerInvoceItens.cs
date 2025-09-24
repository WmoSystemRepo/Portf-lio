public class IntegracaoSppBimerInvoceItens
{
    public int CdEmpresa { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public string NumeroDaNFSe { get; set; } = string.Empty;

    public string CdProduto { get; set; } = string.Empty;
    public string NomeDoProduto { get; set; } = string.Empty;
    public decimal Qtd { get; set; }
    public decimal PrecoUnitItem { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal PrecoVendaUS { get; set; }
    public decimal PrecoVendaReal { get; set; }
    public decimal PrecoCompraUS { get; set; }
    public decimal PrecoCompraReal { get; set; }

    public IntegracaoSppBimerInvoce IntegracaoSppBimerInvoce { get; set; } = null!;
}