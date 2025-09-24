using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Request.Bimer;
using System.Diagnostics;
using System.Text.Json;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Vexpense_Bimer
{
    [ApiController]
    [Route("api/Bimer")]
    [ApiExplorerSettings(GroupName = "1.2-Integração do Vexpenses com o Bimmer: APIs do Bimmer")]
    [Authorize]
    public class BimerController : Controller
    {
        private readonly IIntegracaoBimerService _integracaoBimerService;
        private readonly IConfiguration _configuration;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _logRepository;

        public BimerController(
            IIntegracaoBimerService integracaoBimerService,
            IConfiguration configuration,
            IntegracaoVexpensesBimmerLogErroRepository logRepository)
        {
            _integracaoBimerService = integracaoBimerService;
            _configuration = configuration;
            _logRepository = logRepository;
        }

        [HttpPost("TituloAPagar/criarTituloPagar")]
        public async Task<IActionResult> CreateTitlePay([FromQuery] string token, [FromBody] BimmerRequestRequiredFieldsDto bimerRequestDto, CancellationToken cancellationToken)
        {
            try
            {
                var resultado = await _integracaoBimerService.RegistrarTituloAPagarNoBimmerAsync(bimerRequestDto, token, cancellationToken);
                return Ok(resultado);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(CreateTitlePay),
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
                    Payload = $"{bimerRequestDto}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("Autenticacao/CriarTokenUtilizacaoServico")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _integracaoBimerService.AutenticarNoBimmerAsync(request, cancellationToken);
                if (!string.IsNullOrEmpty(result.error))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Authenticate),
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
                    Payload = $"request={request}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("Autenticacao/ReautenticarTokenUtilizacaoServico")]
        public async Task<IActionResult> Reauthenticate([FromBody] ReauthenticateRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _integracaoBimerService.RenovarAutenticacaoNoBimmerAsync(request, cancellationToken);
                if (!string.IsNullOrEmpty(result.error))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Reauthenticate),
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
                    Payload = $"request={request}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("ObterBimerIdentificadorPessoaPorCpfAsync")]
        public async Task<IActionResult> BuscarIdentificadorPessoa([FromQuery] string cpf, CancellationToken cancellationToken)
        {
            try
            {
                var clientId = _configuration.GetSection("BimmerAuth:client_id").Value;
                var clientSecret = _configuration.GetSection("BimmerAuth:client_secret").Value;
                var grantType = _configuration.GetSection("BimmerAuth:grant_type").Value;
                var username = _configuration.GetSection("BimmerAuth:username").Value;
                var password = _configuration.GetSection("BimmerAuth:password").Value;
                var nonce = _configuration.GetSection("BimmerAuth:nonce").Value;

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(grantType) ||
                    string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nonce))
                {
                    return BadRequest("Configuração de autenticação inválida ou incompleta.");
                }

                AuthResponseDto token = await _integracaoBimerService.AutenticarNoBimmerAsync(new AuthRequestDto
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    GrantType = grantType,
                    Username = username,
                    Password = password,
                    Nonce = nonce,
                });

                var result = await _integracaoBimerService.ObterBimerIdentificadorPessoaPorCpfAsync(cpf, token.access_token);
                if (string.IsNullOrEmpty(result.Identificador))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(BuscarIdentificadorPessoa),
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
                    Payload = $"CPF={cpf}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
    }
}