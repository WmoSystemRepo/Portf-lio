using Newtonsoft.Json;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense
{
    /// <summary>
    /// Representa a resposta da API ao consultar um membro da equipe no VExpenses.
    /// </summary>
    public class ApiResponseTeamMember
    {
        /// <summary>
        /// Caminho da requisição enviada para a API.
        /// </summary>
        [JsonProperty("request")]
        public string? Request { get; set; }

        /// <summary>
        /// Método HTTP utilizado na requisição (GET, POST, etc.).
        /// </summary>
        [JsonProperty("method")]
        public string? Method { get; set; }

        /// <summary>
        /// Indica se a requisição foi executada com sucesso.
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Código de status retornado pela API.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// Mensagem descritiva da resposta da API.
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Objeto contendo os dados do membro da equipe retornado.
        /// </summary>
        [JsonProperty("data")]
        public TeamMember? Data { get; set; }
    }

    /// <summary>
    /// Representa os dados de um colaborador (membro da equipe) integrado no VExpenses.
    /// </summary>
    public class TeamMember
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("integration_id")]
        public int? IntegrationId { get; set; }

        [JsonProperty("external_id")]
        public string? ExternalId { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("role_id")]
        public int? RoleId { get; set; }

        [JsonProperty("approval_flow_id")]
        public int ApprovalFlowId { get; set; }

        [JsonProperty("expense_limit_policy_id")]
        public int ExpenseLimitPolicyId { get; set; }

        [JsonProperty("user_type")]
        public string? UserType { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("cpf")]
        public required string Cpf { get; set; }

        [JsonProperty("phone1")]
        public string? Phone1 { get; set; }

        [JsonProperty("phone2")]
        public string? Phone2 { get; set; }

        [JsonProperty("birth_date")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("bank")]
        public string? Bank { get; set; }

        [JsonProperty("agency")]
        public string? Agency { get; set; }

        [JsonProperty("account")]
        public string? Account { get; set; }

        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("parameters")]
        public string? Parameters { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
