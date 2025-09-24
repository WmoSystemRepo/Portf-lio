using AutoMapper;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations.AnyPoint;
using PARS.Inhouse.Systems.Application.Configurations.Integrcoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Relatorios;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.VexpenssesBimer
{
    public class VExpensesService : IVExpensesService
    {
        private readonly IVExpensesApi _vExpensesApi;
        private readonly OpcoesUrls _options;
        private readonly string _tokenApiKey;
        private readonly IMapper _mapper;

        public VExpensesService(IVExpensesApi vExpensesApi, IOptions<OpcoesUrls> options, IOptions<VexpenseTokenApiKeyConfig> tokenApiKey, IMapper mapper)
        {
            _vExpensesApi = vExpensesApi ?? throw new ArgumentNullException(nameof(vExpensesApi));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _tokenApiKey = tokenApiKey?.Value?.Token ?? throw new ArgumentNullException(nameof(tokenApiKey));
            _mapper = mapper;
        }

        public async Task<List<ReportDto>> ObterRelatorioPorStatusVexpenssesAsync(string status, FiltrosDto filtrosDto, List<IntegracaoBimmerInsercaoPendentes>? listaPendencias = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var token = _tokenApiKey;
                var filtrosDtoPadrao = AplicarFiltrosPadrao(filtrosDto);
                var uri = _options.VExpenseReport.Replace("{status}", status);

                var statusPago = status.ToUpper() == EnumHelper.ObterValorEnumMember(StatusRelatorioVexpensses.PAGO);

                cancellationToken.ThrowIfCancellationRequested();

                IReadOnlyList<ReportDto>? reports = statusPago
                    ? await _vExpensesApi.BuscarRelatorioPorStatusPagoAsync(status, uri, token, cancellationToken)
                    : await _vExpensesApi.BuscarRelatorioPorStatusAsync(status, uri, token, filtrosDtoPadrao, cancellationToken);

                var reportsList = reports.Select(r => new ReportDto
                {
                    id = r.id,
                    external_id = r.external_id,
                    user_id = r.user_id,
                    device_id = r.device_id,
                    description = r.description,
                    status = r.status,
                    approval_stage_id = r.approval_stage_id,
                    approval_user_id = r.approval_user_id,
                    approval_date = r.approval_date,
                    payment_date = r.payment_date,
                    payment_method_id = r.payment_method_id,
                    observation = r.observation,
                    paying_company_id = r.paying_company_id,
                    on = r.on,
                    justification = r.justification,
                    pdf_link = r.pdf_link,
                    excel_link = r.excel_link,
                    created_at = r.created_at,
                    updated_at = r.updated_at,
                    expenses = r.expenses != null ? MapearDtoResponse(r.expenses) : null
                }).ToList();

                if (status != EnumHelper.ObterValorEnumMember(StatusRelatorioVexpensses.APROVADO))
                {
                    reportsList = reportsList
                        .Where(x => x.updated_at >= DateTime.Now.AddDays(-7))
                        .ToList();
                }

                var filteredReports = reportsList
                    .Where(r => listaPendencias == null
                                || !listaPendencias.Any(p => p.IdResponse == r.id))
                    .ToList();

                await SaveReports(filteredReports, status, cancellationToken);

                return filteredReports;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private FiltrosDto AplicarFiltrosPadrao(FiltrosDto filtrosDto)
        {
            return new FiltrosDto
            {
                SearchField = filtrosDto.SearchField != default ? filtrosDto.SearchField : FiltroSearchField.approval_date_between,
                Search = !string.IsNullOrEmpty(filtrosDto.Search) ? filtrosDto.Search : "",
                SearchJoin = filtrosDto.SearchJoin != default ? filtrosDto.SearchJoin : FiltroSearchJoin.and
            };
        }

        private ExpenseContainerDto MapearDtoResponse(ExpenseContainerDto expenseContainer)
        {
            return new ExpenseContainerDto
            {
                data = expenseContainer?.data?.Select(exp => new ExpenseDto
                {
                    id = exp.id,
                    user_id = exp.user_id,
                    expense_id = exp.expense_id,
                    device_id = exp.device_id,
                    integration_id = exp.integration_id,
                    external_id = exp.external_id,
                    mileage = exp.mileage,
                    date = exp.date,
                    expense_type_id = exp.expense_type_id,
                    payment_method_id = exp.payment_method_id,
                    paying_company_id = exp.paying_company_id,
                    course_id = exp.course_id,
                    reicept_url = exp.reicept_url,
                    value = exp.value,
                    title = exp.title,
                    validate = exp.validate,
                    reimbursable = exp.reimbursable,
                    observation = exp.observation,
                    rejected = exp.rejected,
                    on = exp.on,
                    mileage_value = exp.mileage_value,
                    original_currency_iso = exp.original_currency_iso,
                    exchange_rate = exp.exchange_rate,
                    converted_value = exp.converted_value,
                    converted_currency_iso = exp.converted_currency_iso,
                    created_at = exp.created_at,
                    updated_at = exp.updated_at
                }).ToList() ?? new List<ExpenseDto>()
            };
        }

        private string FormatarCampo<T>(T campo) where T : Enum
        {
            return campo.ToString().ToLower().Replace("_", ":");
        }

        private async Task SaveReports(List<ReportDto> reportsList, string status, CancellationToken cancellationToken)
        {
            try
            {
                await _vExpensesApi.SaveChanges(reportsList, status, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(IReadOnlyList<IntegracaoVexpenseTitulosRelatoriosStatusDto> Reports, int TotalCount)> BuscarRelatoriosAsync(int pageNumber, int pageSize, string? status = null, string? search = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var query = await _vExpensesApi.BuscarRelatoriosAsync(pageNumber, pageSize, status, search, cancellationToken);

            return (query.Reports, query.TotalCount);
        }

        public async Task<(bool, string)> AlterarStatusAsync(int id, AlteraStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var uri = _options.VExpenseStatus.Replace("id", id.ToString());
                var token = _tokenApiKey;

                cancellationToken.ThrowIfCancellationRequested();
                var report = await _vExpensesApi.AlterarStatusAsync(request, uri, token, cancellationToken);

                return (report.Item1, report.Item2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao alterar status. Detalhe: {ex.Message}");
            }
        }

        public async Task<(bool, string)> BuscarUsuarioPorIdVexpenssesAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var uri = _options.VExpenseUser.Replace("id", id.ToString());
                var token = _tokenApiKey;

                cancellationToken.ThrowIfCancellationRequested();
                var user = await _vExpensesApi.BuscarUsuarioPorIdAsync(uri, token, cancellationToken);

                return (user.Item1, user.Item2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário por id. Detalhe: {ex.Message}");
            }
        }

        public async Task<ContagensRelatorios> ObterContagensRelatoriosAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await _vExpensesApi.ObterContagensRelatoriosAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}