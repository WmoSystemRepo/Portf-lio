using System.Globalization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.Bacen
{
    /// <summary>
    /// DTO que representa a cotação do dólar conforme retornada pelo Bacen (Banco Central).
    /// Contém os valores de compra, venda e a data/hora da cotação.
    /// </summary>
    public class CotacaoDolarDto
    {
        /// <summary>
        /// Valor de compra do dólar no momento da cotação.
        /// </summary>
        public decimal cotacaoCompra { get; set; }

        /// <summary>
        /// Valor de venda do dólar no momento da cotação.
        /// </summary>
        public decimal cotacaoVenda { get; set; }

        /// <summary>
        /// Data e hora da cotação no formato texto.
        /// Esperado no padrão: "yyyy-MM-dd HH:mm:ss.fff".
        /// </summary>
        public string dataHoraCotacao { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora da cotação convertida para o tipo DateTime.
        /// Essa propriedade é ignorada na serialização JSON.
        /// </summary>
        [JsonIgnore]
        public DateTime DataHoraCotacao =>
            DateTime.ParseExact(dataHoraCotacao, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }
}
