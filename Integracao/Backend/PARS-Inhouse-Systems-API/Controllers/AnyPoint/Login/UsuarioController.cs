using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.User;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.Login
{
    [Route("api/Usuario")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs de Usuários")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Context _context;
        private readonly IUsuarioService _servico;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public UsuarioController(UserManager<ApplicationUser> userManager,
                                 Context context,
                                 IUsuarioService Servico,
                                 IntegracaoVexpensesBimmerLogErroRepository mongoLogErroRepo)
        {
            _userManager = userManager;
            _context = context;
            _servico = Servico;
            _LogErroRepo = mongoLogErroRepo;
        }

        #region Codigo antigo 
        /// <summary>
        /// Retorna a lista de todos os usuários cadastrados.
        /// </summary>
        /// <remarks>
        /// Endpoint para obter todos os usuários com permissões básicas exibidas.
        /// </remarks>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Lista de usuários (Id, UserName, Email, Permissões básicas)</returns>
        [HttpGet("Lista")]
        public async Task<IActionResult> TodosUsuarios(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var users = await _userManager.Users.ToListAsync(cancellationToken);
                return Ok(users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.PodeLer,
                    u.PodeEscrever,
                    u.PodeRemover
                }));
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
                    Endpoint = nameof(TodosUsuarios),
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
        /// Obtém um usuário específico pelo seu Id.
        /// </summary>
        /// <param name="id">Id do usuário.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Usuário encontrado ou NotFound caso não exista.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterUsuario(string id, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
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
                    Endpoint = nameof(ObterUsuario),
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
        /// Cria um novo usuário no sistema.
        /// </summary>
        /// <param name="model">Dados para criação do usuário.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Usuário criado ou mensagem de erro de validação.</returns>

        [HttpPost("Novo")]
        public async Task<IActionResult> CriarUsuario([FromBody] RegisterDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PodeLer = model.PodeLer,
                    PodeEscrever = model.PodeEscrever,
                    PodeRemover = model.PodeRemover,
                    PodeVerConfiguracoes = model.PodeVerConfiguracoes
                };

                cancellationToken.ThrowIfCancellationRequested();
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }
                return CreatedAtAction(nameof(ObterUsuario), new { id = user.Id }, new { user.Id, user.UserName });
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
                    Endpoint = nameof(CriarUsuario),
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
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado.</param>
        /// <param name="model">Dados para atualização.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>NoContent em caso de sucesso, NotFound ou erro em caso de falha.</returns>
        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> AtualizarUsuario(string id, [FromBody] UserEditDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.PodeLer = model.PodeLer;
                user.PodeEscrever = model.PodeEscrever;
                user.PodeRemover = model.PodeRemover;
                user.PodeVerConfiguracoes = model.PodeVerConfiguracoes;

                cancellationToken.ThrowIfCancellationRequested();
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
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
                    Endpoint = nameof(AtualizarUsuario),
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
                    Payload = $"id={id}, request={model}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza dados do próprio perfil do usuário logado.
        /// </summary>
        /// <param name="id">Id do usuário a ser atualizado.</param>
        /// <param name="model">Dados para atualização de perfil.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>NoContent em caso de sucesso, NotFound ou erro em caso de falha.</returns>
        [HttpPut("updateUserViaMyProfile/{id}")]
        public async Task<IActionResult> AtualizarUsuarioViaPerfil(string id, [FromBody] UserEditDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;

                cancellationToken.ThrowIfCancellationRequested();
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
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
                    Endpoint = nameof(AtualizarUsuarioViaPerfil),
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
                    Payload = $"id={id}, request={model}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Exclui um usuário específico.
        /// </summary>
        /// <param name="id">Id do usuário a ser excluído.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>NoContent em caso de sucesso, NotFound ou erro em caso de falha.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(string id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                cancellationToken.ThrowIfCancellationRequested();
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
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
                    Endpoint = nameof(ExcluirUsuario),
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
        #endregion

        #region Controladores de Usuário com Referência à Regra

        [HttpPost]
        [Route("Regra/Novo")]
        public async Task<IActionResult> UsuarioRegraNovo([FromBody] List<UsuarioRegraDto> usuarioRegra, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (usuarioRegra == null || !usuarioRegra.Any())
                    return BadRequest(new { sucesso = false, mensagem = "A lista de regra está vazia ou inválida." });

                await _servico.UsuarioRegraServicoRegistrarAsync(usuarioRegra, cancellationToken);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Regra registradas com sucesso.",
                    quantidade = usuarioRegra.Count
                });
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
                    Endpoint = nameof(UsuarioRegraNovo),
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
                    Payload = $"Lista={usuarioRegra}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Regra/IdReferencia/{UsuarioId}/{RegraId}")]
        public async Task<IActionResult> ObterRegraPorIdReferencia(string UsuarioId, string RegraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var usuario = await _servico.UsuarioRegraServicoBuscaPorIdReferenciaAsync(UsuarioId, RegraId, cancellationToken);

                if (usuario == null)
                {
                    Console.WriteLine($"⚠️ Regra não encontrada com o ID de referência {RegraId}");
                    return NotFound($"Regra com ID de referência {RegraId} não encontrada.");
                }

                return Ok(usuario);
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
                    Endpoint = nameof(UsuarioRegraNovo),
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
                    Payload = $"UsuarioId={UsuarioId}, RegraId={RegraId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Regra/IdUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterRegraPorIdUsuario(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var Regra = await _servico.UsuarioRegraServicoBuscarPorIdMenuAsync(idUsuario, cancellationToken);

                if (Regra == null || !Regra.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma Regra encontrada para o usuario ID {idUsuario}");
                    return NotFound($"Nenhuma Regra encontrada para o usuario com ID {idUsuario}.");
                }

                return Ok(Regra);
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
                    Endpoint = nameof(ObterRegraPorIdUsuario),
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
                    Payload = $"UsuarioId={idUsuario}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Regra/Excluir/Referencia/{UsuarioId}/{RegraId}")]
        public async Task<IActionResult> ExcluirUsuarioRegra(string UsuarioId, string RegraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                var usuarioRegra = await _servico.UsuarioRegraServicoBuscaPorIdReferenciaAsync(UsuarioId, RegraId, cancellationToken);


                if (usuarioRegra == null)
                {
                    return NotFound($"Regra com ID {RegraId} não encontrada.");
                }


                await _servico.UsuarioRegraServicoExcluirAsync(usuarioRegra, cancellationToken);
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
                    Endpoint = nameof(ExcluirUsuarioRegra),
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
                    Payload = $"UsuarioId={UsuarioId}, RegraId={RegraId}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Usuário com Referência à Permissão
        [HttpPost]
        [Route("Permissao/Novo")]
        public async Task<IActionResult> UsuarioPermissaoNovo([FromBody] List<UsuarioPermissaoDto> integracoes, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (integracoes == null || !integracoes.Any())
                    return BadRequest(new { sucesso = false, mensagem = "A lista de integrações está vazia ou inválida." });

                await _servico.UsuarioPermissaoServicoRegistrarAsync(integracoes, cancellationToken);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Integrações registradas com sucesso.",
                    quantidade = integracoes.Count
                });
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
                    Endpoint = nameof(UsuarioPermissaoNovo),
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
                    Payload = $"Lista={integracoes}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Permissao/IdReferencia/{UsuarioId}/{PermissaoId}")]
        public async Task<IActionResult> ObterPermissaoPorIdReferencia(string UsuarioId, int PermissaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _servico.UsuarioPermissaoServicoBuscaPorIdReferenciaAsync(UsuarioId, PermissaoId, cancellationToken);

                if (menu == null)
                {
                    Console.WriteLine($"⚠️ Permissao não encontrada com o ID de referência {PermissaoId}");
                    return NotFound($"Permissao com ID de referência {PermissaoId} não encontrada.");
                }

                return Ok(menu);
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
                    Endpoint = nameof(ObterPermissaoPorIdReferencia),
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
                    Payload = $"UsuarioId={UsuarioId}, PermissaoId={PermissaoId}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Permissao/IdUsuario/{idUsuario}")]
        public async Task<IActionResult> ObterPermissaoPorIdUsuario(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var permissao = await _servico.UsuarioPermissaoServicoBuscarPorIdMenuAsync(idUsuario, cancellationToken);

                if (permissao == null || !permissao.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma Permissao encontrada para o usuario ID {idUsuario}");
                    return NotFound($"Nenhuma Permissao encontrada para o usuario com ID {idUsuario}.");
                }

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
                    Endpoint = nameof(ObterPermissaoPorIdUsuario),
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
                    Payload = $"UsuarioId={idUsuario}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Permissao/Excluir/Referencia/{UsuarioId}/{PermissaoId}")]
        public async Task<IActionResult> ExcluirUsuarioPermissao(string UsuarioId, int PermissaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _servico.UsuarioPermissaoServicoBuscaPorIdReferenciaAsync(UsuarioId, PermissaoId, cancellationToken);

                if (menuIntegracao == null)
                {
                    Console.WriteLine($"⚠️ Permissão não encontrada para exclusão. ID: {PermissaoId}");
                    return NotFound($"Permissão com ID {PermissaoId} não encontrada.");
                }

                await _servico.UsuarioPermissaoServicoExcluirAsync(menuIntegracao, cancellationToken);

                Console.WriteLine($"🗑️ Integração ID {PermissaoId} excluída com sucesso.");
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
                    Endpoint = nameof(ExcluirUsuarioPermissao),
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
                    Payload = $"UsuarioId={UsuarioId}, PermissaoId={PermissaoId}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Usuário com Referência à Integração

        [HttpPost]
        [Route("Integracao/Novo")]
        public async Task<IActionResult> UsuarioIntegracaoNovo([FromBody] List<UsuarioIntegracaoDto> integracoes, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (integracoes == null || !integracoes.Any())
                    return BadRequest(new { sucesso = false, mensagem = "A lista de integrações está vazia ou inválida." });

                await _servico.UsuarioIntegracaoServicoRegistrarAsync(integracoes, cancellationToken);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Integrações registradas com sucesso.",
                    quantidade = integracoes.Count
                });
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
                    Endpoint = nameof(UsuarioIntegracaoNovo),
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
                    Payload = $"Lista={integracoes}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdReferencia/{UsuarioId}/{IntegracaoId}")]
        public async Task<IActionResult> ObterIntegracaoPorIdReferencia(string UsuarioId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _servico.UsuarioIntegracaoServicoBuscaPorIdReferenciaAsync(UsuarioId, IntegracaoId, cancellationToken);

                if (menu == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada com o ID de referência {IntegracaoId}");
                    return NotFound($"Integração com ID de referência {IntegracaoId} não encontrada.");
                }

                return Ok(menu);
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
                    Endpoint = nameof(ObterIntegracaoPorIdReferencia),
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
                    Payload = $"UsuarioId={UsuarioId}, IntegracaoId={IntegracaoId}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdUSuario/{idUsuario}")]
        public async Task<IActionResult> ObterIntegracaoPorIdUsuario(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var integracoes = await _servico.UsuarioIntegracaoServicoBuscarPorIdMenuAsync(idUsuario, cancellationToken);

                if (integracoes == null || !integracoes.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para o usuario ID {idUsuario}");
                    return NotFound($"Nenhuma integração encontrada para o usuario com ID {idUsuario}.");
                }

                return Ok(integracoes);
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
                    Endpoint = nameof(ObterIntegracaoPorIdUsuario),
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
                    Payload = $"UsuarioId={idUsuario}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Integracao/Excluir/Referencia/{UsuarioId}/{IntegracaoId}")]
        public async Task<IActionResult> ExcluirUsuarioIntegracao(string UsuarioId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Busca a integração antes de excluir
                var menuIntegracao = await _servico.UsuarioIntegracaoServicoBuscaPorIdReferenciaAsync(UsuarioId, IntegracaoId, cancellationToken);

                if (menuIntegracao == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada para exclusão. ID: {IntegracaoId}");
                    return NotFound($"Integração com ID {IntegracaoId} não encontrada.");
                }

                // Realiza a exclusão
                await _servico.UsuarioIntegracaoServicoExcluirAsync(menuIntegracao, cancellationToken);

                Console.WriteLine($"🗑️ Integração ID {IntegracaoId} excluída com sucesso.");
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
                    Endpoint = nameof(ExcluirUsuarioIntegracao),
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
                    Payload = $"UsuarioId={UsuarioId}, IntegracaoId={IntegracaoId}"
                }, CancellationToken.None);


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion
    }
}
