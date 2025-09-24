using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses
{
    /// <summary>
    /// Define os filtros disponíveis para inclusão de dados adicionais na busca.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroInclude
    {
        /// <summary>
        /// Incluir despesas (expenses).
        /// </summary>
        [EnumMember(Value = "expenses")]
        expenses,

        /// <summary>
        /// Incluir usuários (users).
        /// </summary>
        [EnumMember(Value = "users")]
        users,

        /// <summary>
        /// Incluir transações (transactions).
        /// </summary>
        [EnumMember(Value = "transactions")]
        transactions
    }
}
