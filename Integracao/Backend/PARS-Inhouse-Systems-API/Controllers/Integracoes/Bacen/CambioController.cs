using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Services.Integracoes.Bacen;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Bacen;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen.ErrosIntegracoes;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Bacen
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3.1-Integração Bacen: APIs Cotação Dolar")]
    [Authorize]
    public class CambioController : ControllerBase
    {
        private readonly CotacaoService _cotacaoService;
        private readonly IIntegracaoBacenLogErroRepository _logRepository;

        public CambioController(CotacaoService cotacaoService, IntegracaoBacenLogErroRepository logRepository)
        {
            _cotacaoService = cotacaoService;
            _logRepository = logRepository;
        }

        [HttpGet("cotacao-dolar")]
        public async Task<IActionResult> GetCotacaoDolar([FromQuery] DateTime dataInicio, DateTime dataFim, string codigoMoeda, CancellationToken cancellationToken)
        {
            try
            {
                var cotacoes = await _cotacaoService.ObterCotacoesPorPeriodoAsync(codigoMoeda, dataInicio, dataFim, cancellationToken);
                return Ok(cotacoes);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoBacenLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(GetCotacaoDolar),
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
                    Payload = $"dataInicio={dataInicio}, dataFim={dataFim}, codigoMoeda={codigoMoeda}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("listagem")]
        public async Task<IActionResult> GetCotacoes(CancellationToken cancellationToken)
        {
            try
            {
                var cotacoes = await _cotacaoService.ListarCotacoes(cancellationToken);
                return Ok(cotacoes);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoBacenLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(GetCotacoes),
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
                    Payload = $"GetCotacoes"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
    }
}
