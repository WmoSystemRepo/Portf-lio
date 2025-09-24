using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers
{
    [Route("api/DeParaMensagem")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Integração SPP com Bimer-De/para Mensagem")]
    [Authorize]
    public class DeParaMensagemController : ControllerBase
    {
        private readonly IDeParaMensagemService _service;
        private readonly IIntegracaoSppBimerInvoceLogErrosRepository _logRepository;

        public DeParaMensagemController(
            IDeParaMensagemService service,
            IIntegracaoSppBimerInvoceLogErrosRepository logRepository)
        {
            _service = service;
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(GetAll),
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
                    Payload = "GetAll"
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{mensagemPadrao}")]
        public async Task<IActionResult> ObterMensagemMapeada(string mensagemPadrao, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.ObterDeparaPorMensagemPadraoAsync(mensagemPadrao);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ObterMensagemMapeada),
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
                    Payload = $"mensagemPadrao={mensagemPadrao}"
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Id/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(GetById),
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
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeParaMensagemDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.MensagemPadrao }, dto);
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Create),
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
                    Payload = JsonConvert.SerializeObject(dto)
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeParaMensagemDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Update),
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
                    Payload = $"id={id}; dto={JsonConvert.SerializeObject(dto)}"
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logRepository.SaveAsync(new IntegracaoSppBimerInvoceLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Delete),
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
                }, cancellationToken);

                return StatusCode(500, ex.Message);
            }
        }
    }
}