using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses
{
    /// <summary>
    /// Define os campos disponíveis para busca na integração VExpenses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroSearchField
    {
        /// <summary>
        /// Filtra por intervalo de datas de aprovação (approval_date_between).
        /// </summary>
        [EnumMember(Value = "approval_date_between")]
        approval_date_between,

        /// <summary>
        /// Filtra por intervalo de datas de pagamento (payment_date_between).
        /// </summary>
        [EnumMember(Value = "payment_date_between")]
        payment_date_between,

        /// <summary>
        /// Filtra pela data de criação (created_at).
        /// </summary>
        [EnumMember(Value = "created_at")]
        created_at
    }
}