using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.AutorizacaoEndpoints;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.Permicoes
{
    [Route("api/Permicoes")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs Permiçoes")]
    public class PermicoesController : ControllerBase
    {
        private readonly IPermicoesService _servico;
        private readonly Context _context;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public PermicoesController(IPermicoesService permissaoService, Context context, IntegracaoVexpensesBimmerLogErroRepository mongoLogErroRepo)
        {
            _servico = permissaoService;
            _context = context;
            _LogErroRepo = mongoLogErroRepo;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var permissoes = await _servico.ObterTodasPermissaoAsync(cancellationToken);
                return Ok(permissoes);
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
                    Payload = string.Empty
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id, CancellationToken cancellationToken)
        {
            try
            {
                var permissao = await _servico.ObterPermissaoPorIdAsync(id, cancellationToken);
                if (permissao == null)
                    return NotFound();
                return Ok(permissao);
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
                    Endpoint = nameof(ObterPorId),
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

        [HttpPost("Novo")]
        public async Task<IActionResult> NovoRegistro(PermicoesDto permicoes, CancellationToken cancellationToken)
        {
            try
            {
                await _servico.CriarPermicoesAsync(permicoes, cancellationToken);
                return CreatedAtAction(nameof(ObterPorId), new { id = permicoes.Id }, permicoes);
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
                    Endpoint = nameof(NovoRegistro),
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
                    Payload = $"request={permicoes}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Atualizar(int id, AnyPointStorePermicoes permissao, CancellationToken cancellationToken)
        {
            try
            {
                if (id != permissao.Id)
                    return BadRequest();

                await _servico.AtualizarPermissaoAsync(permissao, cancellationToken);
                return NoContent();
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
                    Endpoint = nameof(Atualizar),
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
                    Payload = $"id={id}, request={permissao}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/{id}")]
        public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _servico.ObterPermissaoPorIdAsync(id, cancellationToken);
                if (existing == null)
                {
                    return NotFound();
                }

                await _servico.DeletaPermissaoAsync(id, cancellationToken);
                return NoContent();
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
                    Endpoint = nameof(Excluir),
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

        [HttpPost("SetRoleOnPermission")]
        public async Task<IActionResult> SetRoleOnPermission([FromBody] PermissaoRoleDto rolePermission, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(rolePermission.RoleId))
                {
                    return BadRequest("RoleId é obrigatório.");
                }

                //await _servico.AssignRoleToPermissionAsync(rolePermission.RoleId, rolePermission.PermissionId, cancellationToken);
                return Ok();
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
                    Endpoint = nameof(SetRoleOnPermission),
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
                    Payload = $"rolePermission={rolePermission}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("linkUsers")]
        public async Task<IActionResult> LinkUsers([FromBody] PermissoesUsuarioDto userPermission, CancellationToken cancellationToken)
        {
            try
            {
                if (userPermission.UserIds == null || userPermission.UserIds.Length == 0)
                {
                    return BadRequest("UserIds é obrigatório.");
                }

                await _servico.AssignPermissionToUsersAsync(userPermission.UserIds, userPermission.PermissionId, cancellationToken);
                return Ok();
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
                    Endpoint = nameof(LinkUsers),
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
                    Payload = $"userPermission={userPermission}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("GetUsersLinkedOnPermission/{permissionId}")]
        public async Task<IActionResult> GetUsersLinkedOnPermission(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                if (permissionId <= 0)
                    return BadRequest("PermissionId é obrigatório.");

                var usersLinked = await _servico.GetUsersLinkedToPermissionAsync(permissionId, cancellationToken);

                if (usersLinked == null || !usersLinked.Any())
                    return NotFound("Nenhum usuário vinculado a esta permissão foi encontrado.");

                return Ok(usersLinked);
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
                    Endpoint = nameof(GetUsersLinkedOnPermission),
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
                    Payload = $"permissionId={permissionId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("GetUsersForPermission/{permissionId}")]
        public async Task<IActionResult> GetUsersForPermission(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                if (permissionId <= 0)
                    return BadRequest("PermissionId é obrigatório.");

                var usersLinked = await _servico.GetUsersToPermissionAsync(permissionId, cancellationToken);

                if (usersLinked == null || !usersLinked.Any())
                    return NotFound("Nenhum usuário vinculado a esta permissão foi encontrado.");

                return Ok(usersLinked);
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
                    Endpoint = nameof(GetUsersForPermission),
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
                    Payload = $"permissionId={permissionId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("GetRolesLinkedOnPermission/{permissionId}")]
        public async Task<IActionResult> GetRolesLinkedOnPermission(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                if (permissionId <= 0)
                    return BadRequest("PermissionId é obrigatório.");

                var rolesLinked = await _servico.GetRolesLinkedToPermissionAsync(permissionId, cancellationToken);

                if (rolesLinked == null || !rolesLinked.Any())
                    return NotFound("Nenhum papel vinculado a esta permissão foi encontrado.");

                return Ok(rolesLinked);
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
                    Endpoint = nameof(GetRolesLinkedOnPermission),
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
                    Payload = $"permissionId={permissionId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("RemoveUserFromPermission")]
        public async Task<IActionResult> RemoveUserFromPermission([FromBody] PermissoesUsuarioDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto == null || dto.PermissionId <= 0 || string.IsNullOrEmpty(dto.UserId))
                    return BadRequest("PermissionId e UserId são obrigatórios.");

                await _servico.RemoveUserFromPermissionAsync(dto.PermissionId, dto.UserId, cancellationToken);
                return Ok();
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
                    Endpoint = nameof(RemoveUserFromPermission),
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
                    Payload = $"request={dto}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost("RemoveRoleFromPermission")]
        public async Task<IActionResult> RemoveRoleFromPermission([FromBody] PermissoesUsuarioDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.RoleId))
                    return BadRequest("RoleId é obrigatório.");

                //await _servico.RemoveRoleFromPermissionAsync(dto.RoleId, dto.PermissionId, cancellationToken);
                return Ok();
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
                    Endpoint = nameof(RemoveRoleFromPermission),
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
                    Payload = $"request={dto}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém as permissões (endpoints) atribuídas a um usuário específico.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> ObterPermissoesDoUsuario(string userId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var permissions = await _context.AnyPointUserEndpointPermission
                                                .Where(p => p.UserId == userId)
                                                .ToListAsync();

                return Ok(permissions);
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
                    Endpoint = nameof(ObterPermissoesDoUsuario),
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
                    Payload = $"userId={userId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza as permissões (endpoints) de um usuário.  
        /// Remove as permissões atuais e salva as novas permissões fornecidas.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <param name="permissions">Lista de permissões a serem atribuídas</param>
        [HttpPut("user/{userId}")]
        public async Task<IActionResult> AtualizarPermissoesDoUsuario(string userId, [FromBody] List<AnyPointUserEndpointPermission> permissions, CancellationToken cancellationToken)
        {
            try
            {
                if (permissions == null || permissions.Count == 0)
                {
                    return BadRequest("Nenhuma permissão informada.");
                }

                cancellationToken.ThrowIfCancellationRequested();

                // Remove as permissões existentes
                var existingPermissions = _context.AnyPointUserEndpointPermission.Where(p => p.UserId == userId);
                _context.AnyPointUserEndpointPermission.RemoveRange(existingPermissions);

                // Adiciona as novas permissões
                foreach (var permission in permissions)
                {
                    permission.UserId = userId;
                    _context.AnyPointUserEndpointPermission.Add(permission);
                }

                await _context.SaveChangesAsync();

                return Ok();
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
                    Endpoint = nameof(AtualizarPermissoesDoUsuario),
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
                    Payload = $"userId={userId}, permissions={permissions}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
    }
}
