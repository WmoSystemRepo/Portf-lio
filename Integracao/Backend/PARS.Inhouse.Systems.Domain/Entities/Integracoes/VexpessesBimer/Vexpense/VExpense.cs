namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense
{
    public class VExpenseData
    {
        public List<VExpense>? data { get; set; }
    }
    public class VExpense
    {
        public int? id { get; private set; }
        public int? user_id { get; private set; }
        public int? expense_id { get; private set; }
        public int? device_id { get; private set; }
        public int? integration_id { get; private set; }
        public int? external_id { get; private set; }
        public decimal? mileage { get; private set; }
        public DateTime? date { get; private set; }
        public int? expense_type_id { get; private set; }
        public int? payment_method_id { get; private set; }
        public int? paying_company_id { get; private set; }
        public int? course_id { get; private set; }
        public string? receipt_url { get; private set; }
        public int? value { get; private set; }
        public string? title { get; private set; } = string.Empty;
        public string? validate { get; private set; } = string.Empty;
        public bool? reimbursable { get; private set; }
        public string? observation { get; private set; } = string.Empty;
        public int? rejected { get; private set; }
        public bool? on { get; private set; }
        public decimal? mileage_value { get; private set; }
        public string? original_currency_iso { get; private set; } = "BRL";
        public decimal? exchange_rate { get; private set; } = 1;
        public decimal? converted_value { get; private set; }
        public string? converted_currency_iso { get; private set; } = "BRL";
        public DateTime? created_at { get; private set; }
        public DateTime? updated_at { get; private set; }


    }
}
