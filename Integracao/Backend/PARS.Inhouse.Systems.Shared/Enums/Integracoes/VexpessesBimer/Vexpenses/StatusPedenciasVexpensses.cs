using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses
{
    /// <summary>
    /// Define os status possíveis para relatórios na integração VExpenses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusPendenciasVexpenses
    {
        [EnumMember(Value = "PENDENTE-USUARIO")]
        [Description("Status: Pendente por falta de usuário.")]
        PENDENTE_USUARIO,

        [EnumMember(Value = "PENDENTE-VALOR-DESPESA")]
        [Description("Status: Pendente por falta de valor da despesa.")]
        PENDENTE_VALOR_DESPESA,

        [EnumMember(Value = "PENDENTE-ID-TITULO")]
        [Description("Status: Pendente por falta de ID no título.")]
        PENDENTE_ID_TITULO,

        [EnumMember(Value = "PENDENTE-TIPO-PAGAMENTO")]
        [Description("Status: Pendente devido a tipo de pagamento inválido ou não existente no de-para.")]
        PENDENTE_TIPO_PAGAMENTO,

        [EnumMember(Value = "PENDENTE-NATUREZA-LANCAMENTO")]
        [Description("Status: Pendente devido a natureza de lançamento inválida ou não referenciada no de-para.")]
        PENDENTE_NATUREZA_LANCAMENTO,

        [EnumMember(Value = "PENDENTE-CENTRO-CUSTO")]
        [Description("Status: Pendente devido a centro de custo inválido ou não referenciado no de-para.")]
        PENDENTE_CENTRO_CUSTO
    }
}
