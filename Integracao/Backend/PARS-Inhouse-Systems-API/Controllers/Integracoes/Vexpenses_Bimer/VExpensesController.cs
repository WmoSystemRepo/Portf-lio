using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PARS.Inhouse.Systems.Application.Configurations.AnyPoint;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Vexpense;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.ComponentModel;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Vexpense_Bimer
{
    [ApiController]
    [Route("api/Integracao/Vexpesses")]
    [ApiExplorerSettings(GroupName = "1.1-Integração do Vexpenses com o Bimmer: APIs do Vexpenses")]
    [Authorize]
    public class VexpensesController : ControllerBase
    {
        private readonly IVExpensesService _vExpensesService;
        private readonly OpcoesUrls _options;
        private readonly ILogger<VexpensesController> _logger;
        private readonly IIntegracaoBimerService _integracaoBimerService;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public VexpensesController(
            IVExpensesService vExpensesService,
            IOptionsSnapshot<OpcoesUrls> options,
            IIntegracaoBimerService integracaoBimerService,
            ILogger<VexpensesController> logger,
            IntegracaoVexpensesBimmerLogErroRepository LogErroRepo)
        {
            _vExpensesService = vExpensesService ?? throw new ArgumentNullException(nameof(vExpensesService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _integracaoBimerService = integracaoBimerService ?? throw new ArgumentNullException(nameof(integracaoBimerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _LogErroRepo = LogErroRepo;
        }

        [HttpGet("Relatorios")]
        public async Task<IActionResult> BuscarRelatorioPorStatus(
            [FromQuery, DefaultValue(StatusRelatorioVexpensses.APROVADO)] StatusRelatorioVexpensses status,
            [FromQuery] FiltrosDto filtros,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando relatórios com status: {status}", status);

                var listaPendencias = await _integracaoBimerService.ObterTodosRrgistrosTitulosPendentesAsync(cancellationToken);
                string statusString = Enum.GetName(typeof(StatusRelatorioVexpensses), status) ?? StatusRelatorioVexpensses.APROVADO.ToString();

                var reports = await _vExpensesService.ObterRelatorioPorStatusVexpenssesAsync(statusString, filtros, listaPendencias, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                var lista = reports.Where(x => x.external_id != null).ToList();
                if (reports == null)
                {
                    _logger.LogWarning("Nenhum relatório encontrado para o status: {status}", status);
                    return NotFound(new { Message = "Nenhum relatório encontrado." });
                }

                return Ok(reports);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(BuscarRelatorioPorStatus),
                    HttpMethod = HttpContext?.Request?.Method,
                    Path = HttpContext?.Request?.Path.Value,
                    QueryString = HttpContext?.Request?.QueryString.Value,
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ExceptionType = ex.GetType().FullName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"status={status}, filtros={filtros}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPut("alterarStatus")]
        public async Task<IActionResult> AlteraStatusRelatorio(int id, [FromBody] AlteraStatus request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Alterando status do relatório: {relatório}", id);

                var response = await _vExpensesService.AlterarStatusAsync(id, request, cancellationToken);

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(AlteraStatusRelatorio),
                    HttpMethod = HttpContext?.Request?.Method,
                    Path = HttpContext?.Request?.Path.Value,
                    QueryString = HttpContext?.Request?.QueryString.Value,
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ExceptionType = ex.GetType().FullName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"id={id}, request={request}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("buscarUsuario/{id}")]
        public async Task<IActionResult> BuscarUsuario(int id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Buscando usuário: {userId:}", id);

                var response = await _vExpensesService.BuscarUsuarioPorIdVexpenssesAsync(id, cancellationToken);

                return Ok(response.Item2);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(BuscarUsuario),
                    HttpMethod = HttpContext?.Request?.Method,
                    Path = HttpContext?.Request?.Path.Value,
                    QueryString = HttpContext?.Request?.QueryString.Value,
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ExceptionType = ex.GetType().FullName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"id={id}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Listar/Titulos")]
        public async Task<IActionResult> ListarRelatorios([FromQuery] int pageNumber = 1,
                                                          [FromQuery] int pageSize = 10,
                                                          [FromQuery] string? status = null,
                                                          [FromQuery] string? search = null,
                                                          CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest(new { Message = "Os parâmetros pageNumber e pageSize devem ser maiores que zero." });
                }

                var (reports, totalCount) = await _vExpensesService.BuscarRelatoriosAsync(pageNumber, pageSize, status, search, cancellationToken);

                if (reports == null || !reports.Any())
                {
                    _logger.LogWarning("Nenhum relatório encontrado.");
                    return NotFound(new { Message = "Nenhum relatório encontrado." });
                }

                var response = new
                {
                    TotalItems = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    Reports = reports
                };

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ListarRelatorios),
                    HttpMethod = HttpContext?.Request?.Method,
                    Path = HttpContext?.Request?.Path.Value,
                    QueryString = HttpContext?.Request?.QueryString.Value,
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ExceptionType = ex.GetType().FullName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"pageNumber={pageNumber}, pageSize={pageSize}, status={status}, search={search}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Relatorio/KPIs")]
        public async Task<IActionResult> ObterContagensRelatorios(CancellationToken cancellationToken)
        {
            try
            {
                var counts = await _vExpensesService.ObterContagensRelatoriosAsync(cancellationToken);
                return Ok(counts);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ObterContagensRelatorios),
                    HttpMethod = HttpContext?.Request?.Method,
                    Path = HttpContext?.Request?.Path.Value,
                    QueryString = HttpContext?.Request?.QueryString.Value,
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    TraceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier,
                    ExceptionType = ex.GetType().FullName,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = string.Empty
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
    }
}