using System.ComponentModel.DataAnnotations;

public class IntegracaoSppBimerInvoce
{
    public int CdEmpresa { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;
    public string NumeroDaNFSe { get; set; } = string.Empty;
    public int? CdCliente { get; set; }           // era int
    public string NomeDoFabricante { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public string ObsNotaFiscal { get; set; } = string.Empty;
    public int? CDOperacoes { get; set; }         // era int
    public string DescricaoDaOperacao { get; set; } = string.Empty;
    public int? CDCentroDeCusto { get; set; }     // era int
    public string DescricaoDoCentroDeCusto { get; set; } = string.Empty;
    public string? StatusIntegracaoAlterData { get; set; }
    public string? StatusIntegracaoAlterDataObs { get; set; }

    [MaxLength(5)]
    public string? Estoque { get; set; }

    public int? FabricanteId { get; set; }

    public ICollection<IntegracaoSppBimerInvoceItens> Itens { get; set; } = new List<IntegracaoSppBimerInvoceItens>();
    public ICollection<IntegracaoSppBimerInvoceResumo> Resumos { get; set; } = new List<IntegracaoSppBimerInvoceResumo>();
}
