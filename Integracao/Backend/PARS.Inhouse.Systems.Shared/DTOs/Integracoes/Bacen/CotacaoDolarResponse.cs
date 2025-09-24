namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.Bacen
{
    /// <summary>
    /// DTO que representa uma cotação individual de moeda estrangeira,
    /// retornada pela API do Banco Central.
    /// </summary>
    public class CotacaoMoedaDto
    {
        /// <summary>
        /// Valor de compra da moeda no momento da cotação.
        /// </summary>
        public decimal cotacaoCompra { get; set; }

        /// <summary>
        /// Valor de venda da moeda no momento da cotação.
        /// </summary>
        public decimal cotacaoVenda { get; set; }

        /// <summary>
        /// Data e hora da cotação no formato string (ex: "2024-06-01 13:45:00.000").
        /// Esse campo pode ser convertido para DateTime se necessário.
        /// </summary>
        public string dataHoraCotacao { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do boletim de origem da cotação.
        /// Exemplos: "Fechamento", "Abertura", "Comercial".
        /// </summary>
        public string tipoBoletim { get; set; } = string.Empty;
    }

    /// <summary>
    /// Estrutura de resposta padrão da API de cotações do Bacen,
    /// contendo uma lista de cotações para a moeda consultada.
    /// </summary>
    public class CotacaoMoedaResponse
    {
        /// <summary>
        /// Lista de cotações retornadas pela API.
        /// Cada item representa uma cotação em data/hora diferente.
        /// </summary>
        public List<CotacaoMoedaDto> value { get; set; } = new();
    }
}
