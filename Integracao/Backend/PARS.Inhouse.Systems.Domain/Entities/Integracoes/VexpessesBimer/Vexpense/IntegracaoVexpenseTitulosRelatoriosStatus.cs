using System.ComponentModel.DataAnnotations;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense
{

    public class IntegracaoVexpenseTitulosRelatoriosStatus
    {
        [Key]
        public int Id { get; set; }
        public int IdResponse { get; set; }
        public int? User_id { get; set; }
        public int? Device_id { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public int? Approval_stage_id { get; set; }
        public int? Approval_user_id { get; set; }
        public DateTime? Approval_date { get; set; }
        public DateTime? Payment_date { get; set; }
        public int? Payment_method_id { get; set; }
        public string? Observation { get; set; }
        public int? Paying_company_id { get; set; }
        public bool On { get; set; }
        public string? Justification { get; set; }
        public string? Pdf_link { get; set; }
        public string? Excel_link { get; set; }
        public decimal? ExpensesTotalValue { get; set; }
        public string? ExpensesData { get; set; }
        public DateTime? Created_at { get; set; }
    }
}
