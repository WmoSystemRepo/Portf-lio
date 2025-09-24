using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Regras;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.Roles
{
    [Route("api/Regra")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs Regras")]
    [Authorize]
    public class RegraController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _regras;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public RegraController(RoleManager<IdentityRole> roleManager, IntegracaoVexpensesBimmerLogErroRepository mongoLogErroRepo)
        {
            _regras = roleManager;
            _LogErroRepo = mongoLogErroRepo;
        }

        /// <summary>
        /// Lista todas as regras
        /// </summary>
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> ObterTodasRegrasAsync(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _regras.Roles
                .Select(r => new RegraDto
                {
                    Id = r.Id ?? string.Empty,
                    Nome = r.Name ?? string.Empty
                })
                .ToListAsync(cancellationToken);

                return Ok(roles);
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
                    Endpoint = nameof(ObterTodasRegrasAsync),
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

        /// <summary>
        /// Cria uma nova regra
        /// </summary>
        [HttpPost]
        [Route("Nova")]
        public async Task<IActionResult> CriarRegraAsync([FromBody] RegraDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nome))
                {
                    return BadRequest("O nome da regra é obrigatório.");
                }

                if (await _regras.RoleExistsAsync(dto.Nome))
                {
                    return BadRequest("Essa regra já existe.");
                }

                cancellationToken.ThrowIfCancellationRequested();
                var result = await _regras.CreateAsync(new IdentityRole(dto.Nome));

                return result.Succeeded ? Ok() : BadRequest(result.Errors);
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
                    Endpoint = nameof(CriarRegraAsync),
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
                    Payload = $"dto={dto}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém uma regra por Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterRegraPorIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("Regra não encontrada no sistema.");

                cancellationToken.ThrowIfCancellationRequested();

                var role = await _regras.FindByIdAsync(id);

                if (role == null)
                    return NotFound("Regra não encontrada no sistema.");

                var dto = new RegraDto
                {
                    Id = role.Id ?? string.Empty,
                    Nome = role.Name ?? string.Empty
                };

                return Ok(dto);
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
                    Endpoint = nameof(ObterRegraPorIdAsync),
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

        /// <summary>
        /// Atualiza uma regra
        /// </summary>
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> AtualizarRegraAsync(string id, [FromBody] RegraDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("Regra não encontrada no sistema.");

                var role = await _regras.FindByIdAsync(dto.Id);

                if (role == null)
                    return BadRequest("Regra não encontrada no sistema.");

                cancellationToken.ThrowIfCancellationRequested();

                role.Name = dto.Nome;

                var updateResult = await _regras.UpdateAsync(role);

                if (!updateResult.Succeeded)
                    return BadRequest(updateResult.Errors);

                var updatedDto = new RegraDto
                {
                    Id = role.Id ?? string.Empty,
                    Nome = role.Name ?? string.Empty
                };

                return Ok(updatedDto);
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
                    Endpoint = nameof(AtualizarRegraAsync),
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
                    Payload = $"id={id}, dto={dto}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Exclui uma regra
        /// </summary>
        [HttpDelete("Excluir/{id}")]
        public async Task<IActionResult> ExcluirRegraAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _regras.FindByIdAsync(id);

                if (role == null)
                {
                    return NotFound("Regra não encontrada.");
                }

                cancellationToken.ThrowIfCancellationRequested();

                var result = await _regras.DeleteAsync(role);

                return result.Succeeded ? Ok() : BadRequest(result.Errors);
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
                    Endpoint = nameof(ExcluirRegraAsync),
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
    }
}