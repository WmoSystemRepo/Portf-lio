using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.Response;
using PARS.Inhouse.Systems.Domain.Exceptions;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Relatorios;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Vexpense;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PARS.Inhouse.Systems.Infrastructure.APIs.Integracoes.Vexpenses_Bimmer
{
    public class VExpensesApi : IVExpensesApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VExpensesApi> _logger;
        private readonly Context _context;
        private readonly IMapper _mapper;

        public VExpensesApi(HttpClient httpClient, ILogger<VExpensesApi> logger, Context context, IMapper mapper)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusAsync(string status, string uri, string token, FiltrosDto filtros, CancellationToken cancellationToken)
        {
            try
            {
                var queryString = GerarQueryString(filtros);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{uri}?{queryString}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                cancellationToken.ThrowIfCancellationRequested();
                var response = await _httpClient.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao buscar relatórios: {response.StatusCode} - {responseContent}");
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var responseData = result?.data?.Select(static dto => ReportDto.Create(
                    dto.id,
                    dto.external_id,
                    dto.user_id,
                    dto.device_id,
                    dto.description,
                    dto.status,
                    dto.approval_stage_id,
                    dto.approval_user_id,
                    dto.approval_date,
                    dto.payment_date,
                    dto.payment_method_id,
                    dto.observation,
                    dto.paying_company_id,
                    dto.on,
                    dto.justification,
                    dto.pdf_link,
                    dto.excel_link,
                    dto.created_at,
                    dto.updated_at,
                    dto.expenses
                )).ToList() ?? new List<ReportDto>();

                if (status == "APROVADO")
                {
                    var json = JsonConvert.SerializeObject(responseData);

                    //await AtualizarListasAprovados(json);
                }

                return responseData;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Erro na comunicação com a API do VExpenses.");
                throw new BusinessException($"Erro na comunicação com a API: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Erro ao processar resposta JSON da API.");
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar relatórios.");
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        public async Task<IReadOnlyList<ReportDto>> BuscarRelatorioPorStatusPagoAsync(string status, string uri, string token, CancellationToken cancellationToken)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{uri}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                cancellationToken.ThrowIfCancellationRequested();
                var response = await _httpClient.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new BusinessException($"Erro ao buscar relatórios: {response.StatusCode} - {responseContent}");
                }

                var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var responseData = result?.data?.Select(dto => ReportDto.Create(
                    dto.id,
                    dto.external_id,
                    dto.user_id,
                    dto.device_id,
                    dto.description,
                    dto.status,
                    dto.approval_stage_id,
                    dto.approval_user_id,
                    dto.approval_date,
                    dto.payment_date,
                    dto.payment_method_id,
                    dto.observation,
                    dto.paying_company_id,
                    dto.on,
                    dto.justification,
                    dto.pdf_link?.ToString(),
                    dto.excel_link?.ToString(),
                    dto.created_at,
                    dto.updated_at,
                    dto.expenses
                )).ToList() ?? new List<ReportDto>();

                var json = JsonConvert.SerializeObject(responseData);

                return responseData;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Erro na comunicação com a API do VExpenses.");
                throw new BusinessException($"Erro na comunicação com a API: {httpEx.Message}");
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Erro ao processar resposta JSON da API.");
                throw new BusinessException($"Erro ao processar resposta da API: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar relatórios.");
                throw new BusinessException($"Erro inesperado ao buscar relatórios: {ex.Message}");
            }
        }

        public async Task AvaliarListaDeReavaliacao(string responseData, CancellationToken cancellationToken)
        {
            try
            {
                string caminhoPayload = Path.Combine(GetSolutionRootDirectory(), "PARS.Inhouse.Systems.Infrastructure", "ResponseData", "Payload");
                string caminhoArquivoListaAprovado = Path.Combine(caminhoPayload, "ListaDeAprovados.json");
                string caminhoArquivoReavaliacao = Path.Combine(caminhoPayload, "ReavaliacaoAprovados.json");

                var novaListaAprovados = JsonConvert.DeserializeObject<List<VExpenseResponse>>(responseData) ?? new List<VExpenseResponse>();
                List<VExpenseResponse> listaAntiga = new();
                List<VExpenseResponse> listaRemovidos = new();

                if (File.Exists(caminhoArquivoListaAprovado))
                {
                    string jsonSalvo = await File.ReadAllTextAsync(caminhoArquivoListaAprovado);
                    listaAntiga = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonSalvo) ?? new List<VExpenseResponse>();

                    listaRemovidos = listaAntiga.Where(antigo => !novaListaAprovados.Any(novo => novo.id == antigo.id)).ToList();
                }

                if (listaRemovidos.Any())
                {
                    List<VExpenseResponse> listaReavaliacao = new();
                    if (File.Exists(caminhoArquivoReavaliacao))
                    {
                        string jsonReavaliacao = await File.ReadAllTextAsync(caminhoArquivoReavaliacao);
                        listaReavaliacao = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonReavaliacao) ?? new List<VExpenseResponse>();
                    }

                    foreach (var item in listaRemovidos)
                    {
                        if (!listaReavaliacao.Any(reav => reav.id == item.id))
                        {
                            listaReavaliacao.Add(item);
                        }
                    }

                    listaReavaliacao = listaReavaliacao
                        .Where(reav => !novaListaAprovados.Any(aprov => aprov.id == reav.id && JsonConvert.SerializeObject(aprov) == JsonConvert.SerializeObject(reav)))
                        .ToList();

                    var jsonReavaliacaoAtualizada = JsonConvert.SerializeObject(listaReavaliacao, Formatting.Indented, new JsonSerializerSettings
                    {
                        Converters = { new StringEnumConverter() }
                    });

                    await File.WriteAllTextAsync(caminhoArquivoReavaliacao, jsonReavaliacaoAtualizada);
                }

                var jsonAtualizado = JsonConvert.SerializeObject(novaListaAprovados, Formatting.Indented, new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                });

                await File.WriteAllTextAsync(caminhoArquivoListaAprovado, jsonAtualizado);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro ao atualizar lista de aprovados: {ex.Message}");
            }
        }

        public async Task AtualizarListasAprovados(string responseData)
        {
            try
            {
                string caminhoPayload = Path.Combine(GetSolutionRootDirectory(), "PARS.Inhouse.Systems.Infrastructure", "ResponseData", "Payload", "Vexpenses");
                string caminhoArquivoListaPago = Path.Combine(caminhoPayload, "ListaDePagos.json");
                string caminhoArquivoReavaliacao = Path.Combine(caminhoPayload, "ReavaliacaoAprovados.json");

                var novaListaPago = JsonConvert.DeserializeObject<List<VExpenseResponse>>(responseData) ?? new List<VExpenseResponse>();
                List<VExpenseResponse> listaAntiga = new();
                List<VExpenseResponse> listaRemovidos = new();

                if (File.Exists(caminhoArquivoListaPago))
                {
                    string jsonSalvo = await File.ReadAllTextAsync(caminhoArquivoListaPago);
                    listaAntiga = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonSalvo) ?? new List<VExpenseResponse>();

                    List<VExpenseResponse> listaReavaliacao = new();
                    if (File.Exists(caminhoArquivoReavaliacao))
                    {
                        string jsonReavaliacao = await File.ReadAllTextAsync(caminhoArquivoReavaliacao);
                        listaReavaliacao = JsonConvert.DeserializeObject<List<VExpenseResponse>>(jsonReavaliacao) ?? new List<VExpenseResponse>();
                    }

                    var jsonReavaliacaoAtualizada = JsonConvert.SerializeObject(listaReavaliacao, Formatting.Indented, new JsonSerializerSettings
                    {
                        Converters = { new StringEnumConverter() }
                    });

                    await File.WriteAllTextAsync(caminhoArquivoReavaliacao, jsonReavaliacaoAtualizada);
                }

                var jsonAtualizado = JsonConvert.SerializeObject(novaListaPago, Formatting.Indented, new JsonSerializerSettings
                {
                    Converters = { new StringEnumConverter() }
                });

                await File.WriteAllTextAsync(caminhoArquivoListaPago, jsonAtualizado);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Erro ao atualizar lista de aprovados: {ex.Message}");
            }
        }

        private string GerarQueryString(FiltrosDto filtros)
        {
            var queryParams = new Dictionary<string, string?>
            {
                { "include", filtros.Include.ToString() },
                { "search", filtros.Search },
                { "searchField", filtros.SearchField.ToString() },
                { "searchJoin", filtros.SearchJoin.ToString() }
            };

            return string.Join("&", queryParams
                .Where(q => !string.IsNullOrEmpty(q.Value))
                .Select(q => $"{q.Key}={Uri.EscapeDataString(q.Value!)}"));
        }

        private string GetSolutionRootDirectory()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..", ".."));
        }

        public async Task SaveChanges(List<ReportDto> reports, string status, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var titulosParaAdicionar = new List<IntegracaoVexpenseTitulosRelatoriosStatus>();

                foreach (var report in reports)
                {

                    var registroExistente = await _context.IntegracaoVexpenseTitulosRelatoriosStatus
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.IdResponse == report.id);

                    var novoTitulo = _mapper.Map<IntegracaoVexpenseTitulosRelatoriosStatus>(report);
                    novoTitulo.IdResponse = novoTitulo.Id;
                    novoTitulo.Id = registroExistente?.Id ?? 0;

                    novoTitulo.ExpensesTotalValue = report.expenses?.data?.Sum(expense => expense.value) ?? 0;
                    novoTitulo.ExpensesData = report.expenses?.data != null
                        ? JsonConvert.SerializeObject(report.expenses.data)
                        : "[]";

                    if (report.expenses?.data?.Any() == true)
                    {
                        var moedas = report.expenses.data
                            .Select(x => x.original_currency_iso)
                            .Distinct()
                            .Where(x => !string.IsNullOrEmpty(x))
                            .ToList();

                        string moedasString = string.Join("-", moedas);

                        if (!string.IsNullOrEmpty(moedasString) && moedas.Any(m => m != "BRL"))
                        {
                            var pendente = new IntegracaoBimmerInsercaoPendentes
                            {
                                DataCadastro = DateTime.Now,
                                Descricao = report.description,
                                IdResponse = report.id,
                                UserId = report.user_id,
                                Valor = novoTitulo.ExpensesTotalValue,
                                Observacao = "Tipo de Moeda Estrangeiro(s) - " + moedasString
                            };

                            await _context.IntegracaoBimmerInsercaoPendentes.AddAsync(pendente);

                            novoTitulo.Status = "PENDENTE-MOEDA";
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    titulosParaAdicionar.Add(novoTitulo);
                }

                if (titulosParaAdicionar.Any())
                {
                    foreach (var titulo in titulosParaAdicionar)
                    {
                        if (titulo.IdResponse == default)
                        {
                            throw new ArgumentException($"O campo 'idTitulo' está nulo para IdResponse: {titulo.IdResponse}, Descricao: {titulo.Description}");
                        }

                        if (titulo.IdResponse <= 0)
                        {
                            throw new ArgumentException($"O campo 'idTitulo' é inválido (<= 0) para IdResponse: {titulo.IdResponse}, Descricao: {titulo.Description}");
                        }

                        Console.WriteLine($"Processando título: Id={titulo.Id}, IdTitulo={titulo.Id}, IdResponse={titulo.IdResponse}, Descricao={titulo.Description}");

                        if (titulo.Id > 0)
                        {
                            _context.IntegracaoVexpenseTitulosRelatoriosStatus.Update(titulo);
                        }
                        else
                        {
                            await _context.IntegracaoVexpenseTitulosRelatoriosStatus.AddAsync(titulo);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(IReadOnlyList<IntegracaoVexpenseTitulosRelatoriosStatusDto> Reports, int TotalCount)> BuscarRelatoriosAsync(int pageNumber, int pageSize, string? status = null, string? search = null, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var query = _context.IntegracaoVexpenseTitulosRelatoriosStatus.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(r => r.Status == status);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    var formats = new[] { "dd/MM/yyyy" };
                    if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
                    {
                        query = query.Where(r =>
                            r.Approval_date.Value.Date == searchDate.Date);
                    }
                    else
                    {
                        query = query.Where(r =>
                            r.IdResponse.ToString().Contains(search) ||
                            r.Description.Contains(search));
                    }
                }

                int totalCount = await query.CountAsync();

                List<IntegracaoVexpenseTitulosRelatoriosStatus> reports = await query
                    .OrderByDescending(r => r.Approval_date)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dto = _mapper.Map<IReadOnlyList<IntegracaoVexpenseTitulosRelatoriosStatusDto>>(reports);

                return (dto, totalCount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ContagensRelatorios> ObterContagensRelatoriosAsync(CancellationToken cancellationToken)
        {
            try
            {
                return new ContagensRelatorios
                {
                    TotalGeral = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(),
                    TotalAbertos = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "ABERTO"),
                    TotalReabertos = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "REABERTO"),
                    TotalAprovados = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "APROVADO"),
                    TotalReprovados = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "REPROVADO"),
                    TotalEnviados = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "ENVIADO"),
                    TotalPago = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "PAGO"),
                    TotalPendencias = await _context.IntegracaoVexpenseTitulosRelatoriosStatus.CountAsync(r => r.Status == "PENDENTE"),
                    TotalErros = await _context.IntegracaoBimmerHistoricoErrosInsercoes.CountAsync(),
                    TotalSucesso = await _context.IntegracaoBimmerInsertOK.CountAsync(),
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(bool, string)> AlterarStatusAsync(AlteraStatus request, string uri, string token, CancellationToken cancellationToken)
        {
            try
            {
                var jsonPayload = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                cancellationToken.ThrowIfCancellationRequested();
                var response = await _httpClient.PutAsync(uri, content);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var responseContentError = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JsonConvert.DeserializeObject<ApiResponseAlteraStatus?>(responseContentError);

                    var responseJson = JsonConvert.SerializeObject(jsonResponse);
                    return (false, responseJson);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponseData = JsonConvert.DeserializeObject<ApiResponseAlteraStatus?>(responseContent);

                var responseJsonData = JsonConvert.SerializeObject(jsonResponseData);

                return (true, responseJsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao alterar status. Detalhes: {ex.Message}");
            }
        }

        public async Task<(bool, string)> BuscarUsuarioPorIdAsync(string uri, string token, CancellationToken cancellationToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);

                cancellationToken.ThrowIfCancellationRequested();
                var response = await _httpClient.GetAsync(uri);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new BusinessException("Não autorizado para acessar a API.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var responseContentError = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JsonConvert.DeserializeObject<ApiResponseTeamMember?>(responseContentError);

                    var responseJson = JsonConvert.SerializeObject(jsonResponse);
                    return (false, responseJson);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponseData = JsonConvert.DeserializeObject<ApiResponseTeamMember?>(responseContent);

                var responseJsonData = JsonConvert.SerializeObject(jsonResponseData);

                return (true, responseJsonData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao alterar status. Detalhes: {ex.Message}");
            }
        }

        public class ApiResponse
        {
            public List<ReportDto>? data { get; set; }
        }
    }
}
