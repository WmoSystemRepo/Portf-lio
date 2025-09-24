using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses
{
    /// <summary>
    /// Define os operadores lógicos usados na filtragem de buscas na integração VExpenses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FiltroSearchJoin
    {
        /// <summary>
        /// Operador lógico AND (and).
        /// </summary>
        [EnumMember(Value = "and")]
        and,

        /// <summary>
        /// Operador lógico OR (or).
        /// </summary>
        [EnumMember(Value = "or")]
        or
    }
}