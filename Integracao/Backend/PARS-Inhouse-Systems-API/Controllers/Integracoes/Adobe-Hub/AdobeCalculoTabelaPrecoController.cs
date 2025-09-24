using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Diagnostics;

[ApiController]
[Route("api/Adobe")]
public sealed class AdobeCalculoTabelaPrecoController : ControllerBase
{
    private readonly IntegracaoAdobeHubLogErroRepository _LogErroRepo;

    public AdobeCalculoTabelaPrecoController(IntegracaoAdobeHubLogErroRepository LogErroRepo)
    {
        _LogErroRepo = LogErroRepo ?? throw new ArgumentNullException(nameof(LogErroRepo));
    }

    [HttpGet("Configuracoes")]
    public async Task<ActionResult<ConfiguracoesResponseDto>> GetConfigs(
        [FromServices] ConfiguracoesService service,
        [FromQuery] int fabricanteId,
        [FromQuery] string segmento,
        CancellationToken ct)
    {
        try
        {
            var result = await service.Handle(fabricanteId, segmento, ct);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499, "Requisição cancelada pelo cliente.");
        }
        catch (Exception ex)
        {
            await LogErro(nameof(GetConfigs), ex, new { fabricanteId, segmento });
            return StatusCode(500,
                $"Erro ao obter configurações (Fabricante={fabricanteId}, Segmento={segmento}): {ex.Message}");
        }
    }

    [HttpPost("Calcular")]
    public async Task<ActionResult<CalcularPrecoResponseDto>> Calcular(
        [FromServices] CalculoPrecoService useCase,
        [FromBody] CalcularPrecoRequestDto request,
        CancellationToken ct)
    {
        try
        {
            if (request is null || request.Linhas.Count == 0)
                return BadRequest("Requisição vazia.");

            var result = await useCase.Handle(request, ct);
            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499, "Requisição cancelada pelo cliente.");
        }
        catch (Exception ex)
        {
            var linhasCount = request?.Linhas?.Count ?? 0;
            await LogErro(nameof(Calcular), ex, new { request?.FabricanteId, request?.Segmento, linhas = linhasCount });

            return StatusCode(500,
                $"Erro ao calcular preços (Fabricante={request?.FabricanteId}, Segmento={request?.Segmento}, Linhas={linhasCount}): {ex.Message}");
        }
    }

    #region 🔧 Auxiliar de Log
    private async Task LogErro(string endpoint, Exception ex, object? payload = null)
    {
        await _LogErroRepo.SaveAsync(new IntegracaoAdobeHubLogsErros
        {
            DataHoraUtc = DateTime.UtcNow,
            Endpoint = nameof(endpoint),
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
            Payload = $"{payload}"
        }, CancellationToken.None);
    }
    #endregion
}
