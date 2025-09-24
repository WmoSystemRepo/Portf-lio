using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Spp_Bimer_Invoce
{
    [ApiController]
    [Route("api/Reprocessar/Spp/BimerInvoce")]
    [ApiExplorerSettings(GroupName = "4.1-Integração SPP com Bimer-Invoce Apis: Reprocessamento")]
    [Authorize]
    public class ReprocessarSppBimerInvoceController : Controller
    {
        private readonly IIntegracaoBimerService _integracaoBimerService;
        private readonly IConfiguration _configuration;
        private readonly IIntegracaoSppBimerInvoceLogErrosRepository _logRepository;

        public ReprocessarSppBimerInvoceController(
            IIntegracaoBimerService integracaoBimerService,
            IConfiguration configuration,
            IIntegracaoSppBimerInvoceLogErrosRepository logRepository)
        {
            _integracaoBimerService = integracaoBimerService;
            _configuration = configuration;
            _logRepository = logRepository;
        }

        [HttpPost("Reprocesso/Manual")]
        public async Task<IActionResult> InserirTituloManual([FromBody] int[] ids, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var token = await ObterTokenDeAutenticacaoBimmerAsync(cancellationToken);
                if (token == null)
                    return BadRequest("Configurações de autenticação estão incompletas ou ausentes.");

                var result = await _integracaoBimerService.ProcessarEnvioTitulosParaBimmerAsync(
                    token.access_token,
                    true,
                    ids,
                    cancellationToken);

                var resposta = result.Select(x => new
                {
                    Id = x.Item1,
                    Descricao = x.Item2,
                    Integrado = x.Item3,
                    Mensagem = x.Item3 ? "Integrado com sucesso" : "Falha na integração"
                });

                return Ok(resposta);
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
                    Endpoint = nameof(InserirTituloManual),
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
                    Payload = JsonConvert.SerializeObject(ids)
                }, cancellationToken);

                return StatusCode(500, $"Erro ao processar a integração: {ex.Message}");
            }
        }

        [HttpGet("Lista/Erros")]
        public async Task<IActionResult> ObterIntegracaoErros(int pageNumber, int pageSize, string? search)
        {
            try
            {
                // Aqui você pode implementar a busca real de erros no SQL Server se precisar
                return Ok(new { Message = "Endpoint de consulta de erros não implementado no SQL Server." });
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ObterIntegracaoErros),
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
                    Payload = JsonConvert.SerializeObject(new { pageNumber, pageSize, search })
                });

                return StatusCode(500, "Erro ao consultar erros de integração.");
            }
        }

        [HttpGet("Sucesso")]
        public async Task<IActionResult> ObterIntegracaoSucesso([FromQuery] int pageNumber = 1,
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

                var result = await _integracaoBimerService
                    .ObterInsercoesBemSucedidasDeIntegracaoComPaginamentoAsync(pageNumber, pageSize, search, cancellationToken);

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
                    Endpoint = nameof(ObterIntegracaoSucesso),
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
                    Payload = JsonConvert.SerializeObject(new { pageNumber, pageSize, status, search })
                }, cancellationToken);

                return StatusCode(500, $"Erro ao buscar inserções com sucesso integração bimmer: {ex.Message}");
            }
        }

        private async Task<AuthResponseDto?> ObterTokenDeAutenticacaoBimmerAsync(CancellationToken cancellationToken)
        {
            try
            {
                var clientId = _configuration["BimmerAuth:client_id"];
                var clientSecret = _configuration["BimmerAuth:client_secret"];
                var grantType = _configuration["BimmerAuth:grant_type"];
                var username = _configuration["BimmerAuth:username"];
                var password = _configuration["BimmerAuth:password"];
                var nonce = _configuration["BimmerAuth:nonce"];

                if (string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(grantType) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(nonce))
                {
                    return null;
                }

                return await _integracaoBimerService.AutenticarNoBimmerAsync(new AuthRequestDto
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    GrantType = grantType,
                    Username = username,
                    Password = password,
                    Nonce = nonce
                }, cancellationToken);
            }
            catch
            {
                throw;
            }
        }
    }
}