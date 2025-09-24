namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer
{
    /// <summary>
    /// Representa o payload principal para envio de uma requisição Bimer.
    /// Contém dados do título financeiro e seus desmembramentos fiscais.
    /// </summary>
    public class BimerRequestDto
    {
        public string? dataCadastro { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }

        /// <summary>
        /// Lista de itens relacionados ao lançamento financeiro.
        /// </summary>
        public List<Item>? Itens { get; set; }

        public int Valor { get; set; }
        public string? IdentificadorBanco { get; set; }
        public string? IdentificadorCategoriaPessoa { get; set; }
        public string? IdentificadorFormaPagamento { get; set; }
        public string? IdentificadorModalidadePagamento { get; set; }
        public string? CodigoEmpresa { get; set; }
        public string? DataEmissao { get; set; }
        public string? DataReferencia { get; set; }
        public string? DataVencimento { get; set; }
        public string? Descricao { get; set; }

        // Desmembramentos fiscais e contábeis
        public DesmembramentoCOFINS? DesmembramentoCOFINS { get; set; }
        public DesmembramentoCSLL? DesmembramentoCSLL { get; set; }
        public DesmembramentoDesconto? DesmembramentoDesconto { get; set; }
        public DesmembramentoINSS? DesmembramentoINSS { get; set; }
        public DesmembramentoIRRF? DesmembramentoIRRF { get; set; }
        public DesmembramentoISS? DesmembramentoISS { get; set; }
        public DesmembramentoJuros? DesmembramentoJuros { get; set; }
        public DesmembramentoMulta? DesmembramentoMulta { get; set; }
        public DesmembramentoOutros? DesmembramentoOutros { get; set; }
        public DesmembramentoPIS? DesmembramentoPIS { get; set; }
        public DesmembramentoPisCofinsCsll? DesmembramentoPisCofinsCsll { get; set; }

        public string IdentificadorPessoa { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string NumeroAgenciaBancaria { get; set; } = string.Empty;
        public string NumeroCodigoBarra { get; set; } = string.Empty;
        public string NumeroContaBancaria { get; set; } = string.Empty;
        public string NumeroTituloBanco { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public bool Previsao { get; set; }
        public string TipoLiquidacao { get; set; } = string.Empty;
    }

    /// <summary>
    /// Representa um item de lançamento financeiro com possível rateio por centro de custo.
    /// </summary>
    public class Item
    {
        public string IdentificadorNaturezaLancamento { get; set; } = string.Empty;
        public List<CentroCusto>? CentroDeCusto { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public int Valor { get; set; }
    }

    /// <summary>
    /// Representa um centro de custo vinculado a um item, incluindo valor e data de lançamento.
    /// </summary>
    public class CentroCusto
    {
        public int AliquotaPorcentagem { get; set; }
        public string DataLancamento { get; set; } = string.Empty;
        public string IdentificadorCentroDeCusto { get; set; } = string.Empty;
        public int ValorLancamento { get; set; }
    }

    // Abaixo seguem todos os desmembramentos fiscais, documentados de forma genérica:

    /// <summary> Desmembramento de COFINS </summary>
    public class DesmembramentoCOFINS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de CSLL </summary>
    public class DesmembramentoCSLL
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desconto aplicado no título </summary>
    public class DesmembramentoDesconto
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de INSS </summary>
    public class DesmembramentoINSS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de IRRF </summary>
    public class DesmembramentoIRRF
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de ISS </summary>
    public class DesmembramentoISS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de juros </summary>
    public class DesmembramentoJuros
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de multa </summary>
    public class DesmembramentoMulta
    {
        public string? IdentificadorEventoBaixa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de valores diversos </summary>
    public class DesmembramentoOutros
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento de PIS </summary>
    public class DesmembramentoPIS
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }

    /// <summary> Desmembramento unificado de PIS, COFINS e CSLL </summary>
    public class DesmembramentoPisCofinsCsll
    {
        public string? IdentificadorCategoria { get; set; }
        public string? IdetificadorPessoa { get; set; }
        public string? IdentificadorNaturezaLancamento { get; set; }
        public int? Valor { get; set; }
    }
}
