using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.Response
{
    public class VExpenseResponse
    {
        public int? id { get; set; }
        public string? external_id { get; set; }
        public int? user_id { get; set; }
        public int? device_id { get; set; }
        public string description { get; set; } = string.Empty;
        public StatusRelatorioVexpensses status { get; set; }
        public int? approval_stage_id { get; set; }
        public int? approval_user_id { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? approval_date { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? payment_date { get; set; }
        public int? payment_method_id { get; set; }
        public string? observation { get; set; }
        public int? paying_company_id { get; set; }
        public bool on { get; set; }
        public string? justification { get; set; }
        public string? pdf_link { get; set; }
        public string? excel_link { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? created_at { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updated_at { get; set; }
        public ExpenseContainerResponse? expenses { get; set; }

        public VExpenseResponse() { }

        public DateTime? GetPaymentDate()
        {
            if (!string.IsNullOrWhiteSpace(payment_date.ToString()) && DateTime.TryParse(payment_date.ToString(), out var parsedDate))
            {
                return parsedDate;
            }
            return null;
        }

        public DateTime? GetApprovalDate()
        {
            if (!string.IsNullOrWhiteSpace(approval_date.ToString()) && DateTime.TryParse(approval_date.ToString(), out var parsedDate))
            {
                return parsedDate;
            }
            return null;
        }


        public void SetPdfLink(string? pdfLink)
        {
            if (pdfLink == null)
            {
                pdfLink = null;
                return;
            }

            if (!Uri.TryCreate(pdfLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do PDF não é válido.");

            pdfLink = validUri.ToString();
        }

        public void SetExcelLink(string? excelLink)
        {
            if (excelLink == null)
            {
                excelLink = null;
                return;
            }

            if (!Uri.TryCreate(excelLink, UriKind.Absolute, out Uri? validUri))
                throw new ArgumentException("O link do Excel não é válido.");

            excelLink = validUri.ToString();
        }
    }

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