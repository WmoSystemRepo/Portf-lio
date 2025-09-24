namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense
{
    /// <summary>
    /// DTO utilizado para representar o status de um título integrado via VExpenses,
    /// com informações relevantes para geração de relatórios e controle de integração.
    /// </summary>
    public class IntegracaoVexpenseTitulosRelatoriosStatusDto
    {
        /// <summary>
        /// Identificador de resposta da integração (pode vir do VExpenses ou sistema intermediário).
        /// </summary>
        public int IdResponse { get; set; }

        /// <summary>
        /// Descrição geral da despesa ou da operação integrada.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status atual do título (ex: "aprovado", "pendente", "erro").
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Observações ou mensagens retornadas pela integração.
        /// </summary>
        public string? Observation { get; set; }

        /// <summary>
        /// Data de aprovação do título, se aplicável.
        /// </summary>
        public DateTime? Approval_date { get; set; }

        /// <summary>
        /// Valor total da despesa vinculada ao título.
        /// </summary>
        public decimal? ExpensesTotalValue { get; set; }

        /// <summary>
        /// Identificador da empresa pagadora da despesa.
        /// </summary>
        public string? ExpensePayingCompanyId { get; set; }

        /// <summary>
        /// Identificador do tipo de despesa.
        /// </summary>
        public string? ExpenseTypeId { get; set; }
    }
}
