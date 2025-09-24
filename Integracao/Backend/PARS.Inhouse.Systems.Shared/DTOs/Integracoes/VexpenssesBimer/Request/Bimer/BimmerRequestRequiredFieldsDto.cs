namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer
{
    /// <summary>
    /// DTO que representa os campos mínimos obrigatórios para envio de um lançamento financeiro ao Bimer.
    /// </summary>
    public class BimmerRequestRequiredFieldsDto
    {
        /// <summary> Identificador da natureza de lançamento contábil. </summary>
        public string? IdentificadorNaturezaLancamento { get; set; }

        /// <summary> Código da empresa no sistema Bimer. </summary>
        public string? CodigoEmpresa { get; set; }

        /// <summary> Data de emissão do título/documento. </summary>
        public DateTime DataEmissao { get; set; }

        /// <summary> Número do título ou documento fiscal. </summary>
        public string? Numero { get; set; }

        /// <summary> Descrição do lançamento financeiro. </summary>
        public string? Descricao { get; set; }

        /// <summary> Identificador da pessoa (fornecedor ou cliente). </summary>
        public string? IdentificadorPessoa { get; set; }

        /// <summary> Valor total do lançamento. </summary>
        public decimal? Valor { get; set; }

        /// <summary> Indica se é uma previsão (true) ou valor real (false). </summary>
        public bool Previsao { get; set; }

        /// <summary> Data de vencimento do título. </summary>
        public DateTime DataVencimento { get; set; }

        /// <summary> Data de referência contábil (opcional). </summary>
        public DateTime? DataReferencia { get; set; }

        /// <summary> Situação administrativa do lançamento, se aplicável. </summary>
        public string? IdentificadorSituacaoAdministrativa { get; set; }

        /// <summary> Forma de pagamento utilizada no título. </summary>
        public string? IdentificadorFormaPagamento { get; set; }

        /// <summary> Observações complementares para o lançamento. </summary>
        public string? Observacao { get; set; }

        /// <summary> Lista de itens que compõem o lançamento (obrigatória). </summary>
        public required List<ItemDto?> Itens { get; set; }
    }

    /// <summary>
    /// Representa um item de lançamento com possível divisão por centro de custo.
    /// </summary>
    public class ItemDto
    {
        /// <summary> Valor individual do item. </summary>
        public decimal Valor { get; set; }

        /// <summary> Natureza contábil vinculada ao item. </summary>
        public string? IdentificadorNaturezaLancamento { get; set; }

        /// <summary> Observações específicas do item. </summary>
        public string? Observacao { get; set; }

        /// <summary> Lista de centros de custo associados ao item. </summary>
        public List<CentroDeCustoDto>? CentrosDeCusto { get; set; }
    }

    /// <summary>
    /// Representa o centro de custo associado a um item de lançamento, com valores e datas.
    /// </summary>
    public class CentroDeCustoDto
    {
        /// <summary> Identificador do centro de custo. </summary>
        public string? IdentificadorCentroDeCusto { get; set; } = null!;

        /// <summary> Valor alocado neste centro de custo. </summary>
        public decimal ValorLancamento { get; set; }

        /// <summary> Percentual de alocação em relação ao item. </summary>
        public decimal AliquotaPorcentagem { get; set; }

        /// <summary> Data do lançamento no centro de custo. </summary>
        public DateTime DataLancamento { get; set; }
    }
}
