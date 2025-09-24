using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoice;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Spp_Bimer_Invoce
{
    [ApiController]
    [Route("api/Monitoramento/Spp/bimer/Invoce")]
    [ApiExplorerSettings(GroupName = "4-Integração SPP com Bimer-Invoce")]
    [Authorize]
    public class MonitoramentoSppBimerController : ControllerBase
    {
        private readonly IMonitorSppBimerAppService _appService;
        private readonly IIntegracaoSppBimerInvoceLogErrosRepository _logRepository;

        public MonitoramentoSppBimerController(
            IMonitorSppBimerAppService appService,
            IIntegracaoSppBimerInvoceLogErrosRepository logRepository)
        {
            _appService = appService;
            _logRepository = logRepository;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> ListarAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appService.ListarTodosAsync(cancellationToken);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ListarAsync),
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
                    Payload = "ListarAsync"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao listar faturas: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonitoramentoSppBimerInvoceDto>>> Get(
            [FromQuery] string? status,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _appService.ObterMonitoramentosAsync(status, dataInicio, dataFim, cancellationToken);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Get),
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
                    Payload = $"status={status ?? "<null>"}; dataInicio={(dataInicio?.ToString("yyyy-MM-dd") ?? "<null>")}; dataFim={(dataFim?.ToString("yyyy-MM-dd") ?? "<null>")}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao obter monitoramentos: {ex.Message}");
            }
        }

        [HttpPost("Reprocessar")]
        public async Task<IActionResult> ReprocessarAsync([FromBody] ReprocessarBimerRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Pedido) ||
                    string.IsNullOrWhiteSpace(request?.Estoque) ||
                    string.IsNullOrWhiteSpace(request?.Fabricante))
                {
                    return BadRequest("Campos obrigatórios: pedido, estoque e fabricante.");
                }

                var resultado = await _appService.ReprocessarAsync(request, cancellationToken);
                return Ok(resultado);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ReprocessarAsync),
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
                    Payload = $"pedido={request?.Pedido}; fabricante={request?.Fabricante}"
                }, CancellationToken.None);

                return StatusCode(500, ex.Message);
            }
        }
    }
}