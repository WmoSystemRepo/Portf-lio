using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.Response
{
    public class ExpenseContainerResponse
    {
        public List<Expense>? data { get; set; }
    }

    public class Expense
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public int? expense_id { get; set; }
        public int? device_id { get; set; }
        public int? integration_id { get; set; }
        public int? external_id { get; set; }
        public string? mileage { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? date { get; set; }
        public int? expense_type_id { get; set; }
        public int? payment_method_id { get; set; }
        public int? paying_company_id { get; set; }
        public int? course_id { get; set; }
        public string? reicept_url { get; set; }
        public decimal? value { get; set; }
        public string title { get; set; } = string.Empty;
        public string validate { get; set; } = string.Empty;
        public bool? reimbursable { get; set; }
        public string observation { get; set; } = string.Empty;
        public int? rejected { get; set; }
        public bool? on { get; set; }
        public string? mileage_value { get; set; }
        public string original_currency_iso { get; set; } = string.Empty;
        public decimal? exchange_rate { get; set; }
        public decimal? converted_value { get; set; }
        public string? converted_currency_iso { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? created_at { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updated_at { get; set; }

        public Expense()
        {
        }

        public static Expense Create(int id, int? user_id, int? expense_id, int? device_id, int? integration_id, int? external_id, string? mileage, DateTime? date,
            int? expense_type_id, int? payment_method_id, int? paying_company_id, int? course_id, string? receipt_url, decimal? value, string title, string? validade,
            bool? reimbursable, string? observation, int? rejected, bool? on, string? mileage_value, string original_currency_iso,
            decimal? exchange_rate, decimal? converted_value, string converted_currency_iso, DateTime? created_at, DateTime? updated_at)
        {
            var expense = new Expense()
            {
                id = id,
                user_id = user_id,
                expense_id = expense_id,
                device_id = device_id,
                integration_id = integration_id,
                external_id = external_id,
                mileage = mileage,
                date = date ?? DateTime.UtcNow,
                expense_type_id = expense_type_id,
                payment_method_id = payment_method_id,
                paying_company_id = paying_company_id,
                course_id = course_id,
                reicept_url = receipt_url,
                value = value,
                title = title,
                validate = validade ?? string.Empty,
                reimbursable = reimbursable,
                observation = observation ?? string.Empty,
                rejected = rejected,
                on = on,
                mileage_value = mileage_value,
                original_currency_iso = original_currency_iso,
                exchange_rate = exchange_rate,
                converted_value = converted_value,
                converted_currency_iso = converted_currency_iso,
                created_at = created_at,
                updated_at = updated_at
            };

            return expense;
        }
    }
}
