using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense
{
    /// <summary>
    /// Representa um relatório de despesas importado da plataforma Vexpenses.
    /// </summary>
    public class ReportDto
    {
        /// <summary>Identificador interno do relatório.</summary>
        public int? id { get; set; }

        /// <summary>Identificador externo da plataforma Vexpenses.</summary>
        public string? external_id { get; set; }

        /// <summary>ID do usuário associado ao relatório.</summary>
        public int? user_id { get; set; }

        /// <summary>ID do dispositivo usado na criação do relatório.</summary>
        public int? device_id { get; set; }

        /// <summary>Descrição geral do relatório.</summary>
        public string description { get; set; } = string.Empty;

        /// <summary>Status atual do relatório.</summary>
        public StatusRelatorioVexpensses status { get; set; }

        /// <summary>Etapa de aprovação atual.</summary>
        public int? approval_stage_id { get; set; }

        /// <summary>ID do usuário responsável pela aprovação.</summary>
        public int? approval_user_id { get; set; }

        /// <summary>Data da aprovação.</summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? approval_date { get; set; }

        /// <summary>Data do pagamento.</summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? payment_date { get; set; }

        /// <summary>ID do método de pagamento utilizado.</summary>
        public int? payment_method_id { get; set; }

        /// <summary>Observações adicionais sobre o relatório.</summary>
        public string? observation { get; set; }

        /// <summary>ID da empresa que realizou o pagamento.</summary>
        public int? paying_company_id { get; set; }

        /// <summary>Indica se o relatório está ativo.</summary>
        public bool on { get; set; }

        /// <summary>Justificativa para status ou alterações.</summary>
        public string? justification { get; set; }

        /// <summary>Link direto para o PDF do relatório.</summary>
        public string? pdf_link { get; set; }

        /// <summary>Link direto para o Excel do relatório.</summary>
        public string? excel_link { get; set; }

        /// <summary>Data de criação do relatório.</summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? created_at { get; set; }

        /// <summary>Data da última atualização do relatório.</summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updated_at { get; set; }

        /// <summary>Lista de despesas associadas ao relatório.</summary>
        public ExpenseContainerDto? expenses { get; set; }

        /// <summary>Construtor padrão.</summary>
        public ReportDto() { }

        /// <summary>Cria uma nova instância preenchida de ReportDto.</summary>
        public static ReportDto Create(
            int? id, string? externalId, int? userId, int? deviceId, string? description,
            StatusRelatorioVexpensses status, int? approvalStageId, int? approvalUserId,
            DateTime? approvalDate, DateTime? paymentDate, int? paymentMethodId,
            string? observation, int? payingCompanyId, bool on, string? justification,
            string? pdfLink, string? excelLink, DateTime? createdAt, DateTime? updatedAt,
            ExpenseContainerDto? expenses)
        {
            var report = new ReportDto
            {
                id = id ?? 0,
                external_id = externalId,
                user_id = userId,
                device_id = deviceId,
                description = description ?? string.Empty,
                status = status,
                approval_stage_id = approvalStageId,
                approval_user_id = approvalUserId,
                approval_date = approvalDate,
                payment_date = paymentDate,
                payment_method_id = paymentMethodId,
                observation = observation,
                paying_company_id = payingCompanyId,
                on = on,
                justification = justification,
                created_at = createdAt,
                updated_at = updatedAt,
                expenses = expenses ?? new ExpenseContainerDto()
            };

            report.SetPdfLink(pdfLink);
            report.SetExcelLink(excelLink);

            return report;
        }

        /// <summary>Define uma nova descrição para o relatório.</summary>
        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");
            this.description = description;
        }

        /// <summary>Define o link para o PDF, validando o formato.</summary>
        public void SetPdfLink(string? pdfLink)
        {
            if (!string.IsNullOrEmpty(pdfLink) && !Uri.TryCreate(pdfLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do PDF não é válido.");
            pdf_link = pdfLink;
        }

        /// <summary>Define o link para o Excel, validando o formato.</summary>
        public void SetExcelLink(string? excelLink)
        {
            if (!string.IsNullOrEmpty(excelLink) && !Uri.TryCreate(excelLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do Excel não é válido.");
            excel_link = excelLink;
        }

        /// <summary>Atualiza o status do relatório com verificação de redundância.</summary>
        public void UpdateStatus(StatusRelatorioVexpensses newStatus)
        {
            if (newStatus == status)
                throw new InvalidOperationException("O novo status deve ser diferente do status atual.");
            status = newStatus;
        }
    }

    /// <summary>
    /// Conversor customizado para datas no formato "yyyy-MM-dd HH:mm:ss".
    /// </summary>
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String &&
                DateTime.TryParse(reader.GetString(), out DateTime date))
            {
                return date;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                writer.WriteNullValue();
        }
    }
}
