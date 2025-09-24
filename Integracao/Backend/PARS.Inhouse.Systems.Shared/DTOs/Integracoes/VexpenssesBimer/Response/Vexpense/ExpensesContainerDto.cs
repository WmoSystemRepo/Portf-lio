using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense
{
    /// <summary>
    /// Container para uma lista de despesas retornadas pela API.
    /// </summary>
    public class ExpenseContainerDto
    {
        /// <summary>
        /// Lista de objetos de despesas.
        /// </summary>
        [JsonPropertyName("data")]
        public List<ExpenseDto>? data { get; set; } = new();
    }

    /// <summary>
    /// Representa os detalhes de uma despesa individual do VExpenses.
    /// </summary>
    public class ExpenseDto
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

        /// <summary>
        /// Construtor padrão obrigatório para serialização.
        /// </summary>
        public ExpenseDto() { }

        /// <summary>
        /// Fábrica para criação de uma despesa completa com dados obrigatórios e opcionais.
        /// </summary>
        public static ExpenseDto Create(int id, int? user_id, int? expense_id, int? device_id, int? integration_id, int? external_id,
            string? mileage, DateTime? date, int? expense_type_id, int? payment_method_id, int? paying_company_id, int? course_id,
            string? receipt_url, decimal? value, string title, string? validade, bool? reimbursable, string? observation, int? rejected,
            bool? on, string? mileage_value, string original_currency_iso, decimal? exchange_rate, decimal? converted_value,
            string converted_currency_iso, DateTime? created_at, DateTime? updated_at)
        {
            return new ExpenseDto
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
        }

        /// <summary>
        /// Define o título da despesa. Obrigatório.
        /// </summary>
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("O título da despesa não pode estar vazio.");
            this.title = title;
        }

        /// <summary>
        /// Define o valor da despesa (deve ser maior que zero).
        /// </summary>
        public void SetValue(int? value)
        {
            if (value <= 0)
                throw new ArgumentException("O valor da despesa deve ser maior que zero.");
            this.value = value;
        }

        /// <summary>
        /// Define a observação da despesa.
        /// </summary>
        public void SetObservation(string? observation)
        {
            this.observation = observation ?? string.Empty;
        }

        /// <summary>
        /// Define a URL do recibo (se válida).
        /// </summary>
        public void SetReceiptUrl(string? url)
        {
            if (url != null && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("A URL do recibo não é válida.");
            reicept_url = url;
        }

        /// <summary>
        /// Define a taxa de câmbio e converte o valor original.
        /// </summary>
        public void SetExchangeRate(decimal exchangeRate, string originalCurrency, string convertedCurrency)
        {
            if (exchangeRate <= 0)
                throw new ArgumentException("A taxa de câmbio deve ser maior que zero.");

            exchange_rate = exchangeRate;
            original_currency_iso = originalCurrency;
            converted_currency_iso = convertedCurrency;
            converted_value = value * exchangeRate;
        }

        /// <summary>
        /// Fábrica alternativa para criação da despesa com alguns tipos alternativos.
        /// </summary>
        public static ExpenseDto Create(int id, int? userId, int? expenseId, int? deviceId, int? integrationId, int? externalId,
            decimal? mileage, DateTime? date, int? expenseTypeId, int? paymentMethodId, int? payingCompanyId, int? courseId,
            string? receiptUrl, int? value, string? title, string? validate, bool? reimbursable, string? observation,
            int? rejected, bool? on, decimal? mileageValue, string? originalCurrencyIso, decimal? exchangeRate,
            decimal? convertedValue, string? convertedCurrencyIso, DateTime? createdAt, DateTime? updatedAt)
        {
            return new ExpenseDto
            {
                id = id,
                user_id = userId,
                expense_id = expenseId,
                value = value,
                title = title ?? string.Empty,
                expense_type_id = expenseTypeId,
                payment_method_id = paymentMethodId,
                paying_company_id = payingCompanyId,
                reimbursable = reimbursable,
                date = date ?? DateTime.UtcNow,
                observation = observation ?? string.Empty,
                reicept_url = receiptUrl,
                exchange_rate = exchangeRate,
                original_currency_iso = originalCurrencyIso ?? string.Empty,
                converted_currency_iso = convertedCurrencyIso,
                created_at = createdAt,
                updated_at = updatedAt
            };
        }
    }
}
