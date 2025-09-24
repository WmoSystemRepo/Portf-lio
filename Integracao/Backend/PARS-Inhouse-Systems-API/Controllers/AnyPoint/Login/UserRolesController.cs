using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.User;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.Login
{
    [Route("api/user-roles")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs de Usuários por Regra")]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public UserRolesController(
            UserManager<ApplicationUser> userManager,
            IntegracaoVexpensesBimmerLogErroRepository LogErroRepo)
        {
            _userManager = userManager;
            _LogErroRepo = LogErroRepo;
        }

        /// <summary>
        /// Lista todos os usuários que possuem uma determinada role.
        /// </summary>
        /// <param name="regraNome">Nome da role.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Lista de usuários que pertencem à role informada.</returns>
        [HttpGet("obter-usuarios-por-regra/{regraNome}")]
        [Authorize]
        public async Task<IActionResult> ObterUsuariosPorRegra(string regraNome, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var users = await _userManager.GetUsersInRoleAsync(regraNome);
                return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
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
                    Endpoint = nameof(ObterUsuariosPorRegra),
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
                    Payload = $"regraNome={regraNome}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Vincula um usuário a uma role.
        /// </summary>
        /// <param name="model">Informações do usuário e da role.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost("vincular-usuario-a-regra")]
        public async Task<IActionResult> VincularUsuarioARegra([FromBody] UserRoleDto model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("Usuário e role são obrigatórios.");

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId!);
                if (user == null)
                    return NotFound("Usuário não encontrado.");

                cancellationToken.ThrowIfCancellationRequested();

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                return result.Succeeded ? Ok("Usuário vinculado à role com sucesso.") : BadRequest(result.Errors);
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
                    Endpoint = nameof(VincularUsuarioARegra),
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
                    Payload = $"request={model}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove um usuário de uma role.
        /// </summary>
        /// <param name="model">Informações do usuário e da role.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost("remover-usuario-da-regra")]
        public async Task<IActionResult> RemoverUsuarioARegra([FromBody] UserRoleDto model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("Usuário e role são obrigatórios.");

            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId!);
                if (user == null)
                    return NotFound("Usuário não encontrado.");

                cancellationToken.ThrowIfCancellationRequested();

                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                return result.Succeeded ? Ok("Usuário removido da role com sucesso.") : BadRequest(result.Errors);
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
                    Endpoint = nameof(RemoverUsuarioARegra),
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
                    Payload = $"request={model}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
    }
}
