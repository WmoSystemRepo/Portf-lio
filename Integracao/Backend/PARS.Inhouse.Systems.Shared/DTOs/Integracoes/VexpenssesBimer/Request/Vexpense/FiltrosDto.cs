using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense
{
    /// <summary>
    /// DTO utilizado para configurar filtros ao consultar dados da API do VExpenses.
    /// Permite selecionar campos, critérios de pesquisa e operadores de junção.
    /// </summary>
    public class FiltrosDto
    {
        /// <summary>
        /// Define quais dados devem ser incluídos na consulta (ex: "expenses", "approvals").
        /// Valor padrão: expenses.
        /// </summary>
        [DefaultValue(FiltroInclude.expenses)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroInclude Include { get; set; } = FiltroInclude.expenses;

        /// <summary>
        /// Valor utilizado na busca textual (ex: nome, descrição, etc).
        /// Pode ser nulo ou vazio se não houver termo específico.
        /// </summary>
        [DefaultValue("")]
        public string? Search { get; set; } = "";

        /// <summary>
        /// Campo que será aplicado no filtro de busca.
        /// Valor padrão: approval_date_between.
        /// </summary>
        [DefaultValue(FiltroSearchField.approval_date_between)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchField SearchField { get; set; } = FiltroSearchField.approval_date_between;

        /// <summary>
        /// Tipo de junção entre filtros (ex: "and" para todos, "or" para qualquer um).
        /// Valor padrão: and.
        /// </summary>
        [DefaultValue(FiltroSearchJoin.and)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FiltroSearchJoin SearchJoin { get; set; } = FiltroSearchJoin.and;

        /// <summary>
        /// Constrói a string de filtro no padrão esperado pela API VExpenses.
        /// Exemplo de retorno: "searchFields=approval_date:between&searchJoin=and".
        /// </summary>
        public string ConstruirFiltro()
        {
            return $"searchFields={FormatarCampo(SearchField)}&searchJoin={FormatarCampo(SearchJoin)}";
        }

        /// <summary>
        /// Converte enums para formato utilizado em parâmetros de URL (minúsculas e com ':' ao invés de '_').
        /// </summary>
        private string FormatarCampo<T>(T campo) where T : Enum
        {
            var nome = campo.ToString().ToLower();
            return nome.Replace("_", ":");
        }
    }
}
