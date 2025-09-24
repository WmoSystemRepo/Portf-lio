using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses
{
    /// <summary>
    /// Define os status possíveis para relatórios na integração VExpenses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusRelatorioVexpensses
    {
        /// <summary>
        /// O relatório está aberto.
        /// </summary>
        [EnumMember(Value = "ABERTO")]
        [Description("Relatório em aberto.")]
        ABERTO,

        /// <summary>
        /// O relatório foi aprovado.
        /// </summary>
        [EnumMember(Value = "APROVADO")]
        [Description("Relatório aprovado.")]
        APROVADO,

        /// <summary>
        /// O relatório foi reprovado.
        /// </summary>
        [EnumMember(Value = "REPROVADO")]
        [Description("Relatório reprovado.")]
        REPROVADO,

        /// <summary>
        /// O relatório foi reaberto.
        /// </summary>
        [EnumMember(Value = "REABERTO")]
        [Description("Relatório reaberto.")]
        REABERTO,

        /// <summary>
        /// O relatório foi pago.
        /// </summary>
        [EnumMember(Value = "PAGO")]
        [Description("Relatório pago.")]
        PAGO,

        /// <summary>
        /// O relatório foi enviado.
        /// </summary>
        [EnumMember(Value = "ENVIADO")]
        [Description("Relatório enviado.")]
        ENVIADO,

        /// <summary>
        /// O relatório foi enviado.
        /// </summary>
        [EnumMember(Value = "APROVADOO")]
        [Description("Relatório aprovado.")]
        APROVADOO,
    }

}
