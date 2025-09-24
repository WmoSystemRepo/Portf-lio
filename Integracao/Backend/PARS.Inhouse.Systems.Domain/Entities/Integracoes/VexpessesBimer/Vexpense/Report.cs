using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.Response;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense
{
    public class Report
    {
        public int Id { get; private set; }
        public string? ExternalId { get; private set; }
        public int? UserId { get; private set; }
        public int? DeviceId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public StatusRelatorioVexpensses Status { get; private set; }
        public int? ApprovalStageId { get; private set; }
        public int? ApprovalUserId { get; private set; }
        public DateTime? ApprovalDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public int? PaymentMethodId { get; private set; }
        public string? Observation { get; private set; }
        public int? PayingCompanyId { get; private set; }
        public bool On { get; private set; }
        public string? Justification { get; private set; }
        public string? PdfLink { get; private set; }
        public string? ExcelLink { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public ExpenseContainerResponse Expenses { get; private set; }

        private Report(string description, StatusRelatorioVexpensses status)
        {
            SetDescription(description);
            Status = status;
            Expenses = new ExpenseContainerResponse();
        }

        public static Report Create(int? id, string? externalId, int? userId, int? deviceId, string? description, StatusRelatorioVexpensses status,
            int? approvalStageId, int? approvalUserId, DateTime? approvalDate, DateTime? paymentDate, int? paymentMethodId,
            string? observation, int? payingCompanyId, bool on, string? justification, string? pdfLink, string? excelLink,
            DateTime? createdAt, DateTime? updatedAt, ExpenseContainerResponse? expenses)
        {
            var report = new Report(description ?? "Relatório Sem Nome", status)
            {
                Id = id ?? 0,
                ExternalId = externalId,
                UserId = userId,
                DeviceId = deviceId,
                ApprovalStageId = approvalStageId,
                ApprovalUserId = approvalUserId,
                ApprovalDate = approvalDate,
                PaymentDate = paymentDate,
                PaymentMethodId = paymentMethodId,
                Observation = observation,
                PayingCompanyId = payingCompanyId,
                On = on,
                Justification = justification,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                Expenses = expenses ?? new ExpenseContainerResponse()
            };

            report.SetPdfLink(pdfLink);
            report.SetExcelLink(excelLink);

            return report;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do relatório não pode estar vazia.");

            Description = description;
        }

        public void SetPdfLink(string? pdfLink)
        {
            if (!string.IsNullOrEmpty(pdfLink) && !Uri.TryCreate(pdfLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do PDF não é válido.");

            PdfLink = pdfLink;
        }

        public void SetExcelLink(string? excelLink)
        {
            if (!string.IsNullOrEmpty(excelLink) && !Uri.TryCreate(excelLink, UriKind.Absolute, out _))
                throw new ArgumentException("O link do Excel não é válido.");

            ExcelLink = excelLink;
        }

        public void UpdateStatus(StatusRelatorioVexpensses newStatus)
        {
            if (newStatus == Status)
                throw new InvalidOperationException("O novo status deve ser diferente do status atual.");

            Status = newStatus;
        }
    }
}