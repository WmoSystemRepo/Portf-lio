using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Menu;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu;
using System.Security.Claims;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.Menu
{
    [Route("api/Menu")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs Menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _servico;
        private readonly IMapper _mapper;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _mongoLogErroRepo;

        public MenuController(IMenuService menuService, IMapper mapper, IntegracaoVexpensesBimmerLogErroRepository mongoLogErroRepo)
        {
            _servico = menuService;
            _mapper = mapper;
            _mongoLogErroRepo = mongoLogErroRepo;
        }

        #region Controladores do Menu
        [HttpGet("Lista")]
        public async Task<IActionResult> ListaMenu(CancellationToken cancellationToken)

        {
            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                cancellationToken.ThrowIfCancellationRequested();
                if (userId == null)
                {
                    return Unauthorized("User ID is null");
                }

                var menus = await _servico.MenuServicoListasAsync(userId, cancellationToken);
                return Ok(menus);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ListaMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = nameof(ListaMenu)
                });


                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Novo")]
        public async Task<IActionResult> NovoMenu(MenuDto menu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _servico.MenuServicoRegistrarAsync(menu, cancellationToken);
                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Menu criado com sucesso.",
                    dados = menu
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "NovoMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = nameof(menu)
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("IdMenu/{idReferencia}")]
        public async Task<IActionResult> BuscarMenuPorId(int idReferencia, CancellationToken cancellationToken)
        {
            try
            {
                var menu = await _servico.MenuServicoBuscaPorIdAsync(idReferencia, cancellationToken);
                if (menu == null)
                    return NotFound();
                return Ok(menu);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "NovoMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = nameof(idReferencia)
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpPut("Editar/{idMenu}")]
        public async Task<IActionResult> EditarMenu(int idMenu, MenuDto menu, CancellationToken cancellationToken)
        {
            try
            {
                if (idMenu != menu.Id)
                    return BadRequest();

                var map = _mapper.Map<AnyPointStoreMenu>(menu);

                cancellationToken.ThrowIfCancellationRequested();
                await _servico.MenuServicoEditarAsync(map, cancellationToken);
                return NoContent();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "EditarMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdMenu={idMenu}, request={menu}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/{idMenu}")]
        public async Task<IActionResult> ExcluirMenu(int idMenu, CancellationToken cancellationToken)
        {
            try
            {

                cancellationToken.ThrowIfCancellationRequested();
                var menu = await _servico.MenuServicoBuscaPorIdAsync(idMenu, cancellationToken);
                if (menu == null)
                    return NotFound();

                cancellationToken.ThrowIfCancellationRequested();
                await _servico.MenuServicoDeletarAsync(menu, cancellationToken);
                return NoContent();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ExcluirMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdMenu={idMenu}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("{id}/referencias")]
        public async Task<IActionResult> ObterTodasReferenciasPorIdMenu(int id, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var referencias = await _servico.MenuServicoObterReferenciasAsync(id, cancellationToken);

                if (referencias == null)
                {
                    Console.WriteLine($"⚠️ Nenhuma Referência encontrada para o menu ID {id}");
                    return NotFound($"Nenhuma Referência encontrada para o menu ID {id}.");
                }

                return Ok(referencias);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterTodasReferenciasPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdMenu={id}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Menu com Referência à Regra

        [HttpPost]
        [Route("Regra/Novo")]
        public async Task<IActionResult> MenuRegraNovo([FromBody] List<MenuRegraDto> menuRegra, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menuRegra == null || !menuRegra.Any())
                    return BadRequest(new
                    {
                        sucesso = false,
                        mensagem = "A lista de regras está vazia ou inválida."
                    });

                await _servico.MenuRegraServicoRegistrarAsync(menuRegra, cancellationToken);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Regras registradas com sucesso.",
                    quantidade = menuRegra.Count
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "MenuRegraNovo",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"request={menuRegra}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Regra/IdMenu/{idMenu}")]
        public async Task<IActionResult> ObterMenuRegraPorIdMenu(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var permissoes = await _servico.MenuRegraServicoBuscarPorIdMenuAsync(idMenu, cancellationToken);

                if (permissoes == null || !permissoes.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma permissão encontrada para o menu ID {idMenu}");
                    return NotFound($"Nenhuma permissão encontrada para o menu ID {idMenu}.");
                }

                return Ok(permissoes);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuRegraPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdMenu={idMenu}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Regra/IdReferencia/{MenuId}/{RegraId}")]
        public async Task<IActionResult> ObterMenuRegraPorIdReferencia(int MenuId, string RegraId, CancellationToken cancellationToken)
        {
            try
            {
                var menu = await _servico.MenuRegraServicoBuscaPorIdReferenciaAsync(MenuId, RegraId, cancellationToken);

                if (menu == null)
                {
                    return NotFound();
                }

                return Ok(menu);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuRegraPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={MenuId}, RegraId={RegraId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Regra/Excluir/Referencia/{MenuId}/{RegraId}")]
        public async Task<IActionResult> ExcluirMenuRegra(int MenuId, string RegraId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuRegra = await _servico.MenuRegraServicoBuscaPorIdReferenciaAsync(MenuId, RegraId, cancellationToken);

                if (menuRegra == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada para exclusão. ID: {RegraId}");
                    return NotFound($"Integração com ID {RegraId} não encontrada.");
                }

                await _servico.MenuRegraServicoDeletetarAsync(menuRegra, cancellationToken);

                Console.WriteLine($"🗑️ Integração ID {RegraId} excluída com sucesso.");
                return NoContent();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuRegraPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={MenuId}, RegraId={RegraId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Menu com Referência à Usuário    
        [HttpPost]
        [Route("Usuario/Novo")]
        public async Task<IActionResult> MenuUsuarioNovo([FromBody] List<MenuUsuarioDto> menuUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (menuUsuario == null || !menuUsuario.Any())
                    return BadRequest(new
                    {
                        sucesso = false,
                        mensagem = "A lista de permissões está vazia ou inválida."
                    });

                await _servico.MenuUsuarioServicoResgistrarAsync(menuUsuario, cancellationToken);

                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Permissões registradas com sucesso.",
                    quantidade = menuUsuario.Count
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "MenuUsuarioNovo",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"request={menuUsuario}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Usuario/IdReferencia/{MenuId}/{UsuarioId}")]
        public async Task<IActionResult> ObterMenuUsuarioPorIdReferencia(int menuId, string UsuarioId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _servico.MenuUsuarioServicoBuscaPorIdReferenciaAsync(menuId, UsuarioId, cancellationToken);

                if (menu == null)
                {
                    Console.WriteLine($"⚠️ Permissão não encontrada para o ID de referência {UsuarioId}");
                    return NotFound($"Permissão com ID de referência {UsuarioId} não encontrada.");
                }

                return Ok(menu);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuUsuarioPorIdReferencia",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={menuId}, RegraId={UsuarioId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Usuario/IdUsuario/{idUsuario}")]
        public async Task<IActionResult> MenuUsuarioServicoBuscaPorIdUaurioAsync(string idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _servico.MenuUsuarioServicoBuscaPorIdUsuarioAsync(idUsuario, cancellationToken);

                if (menu == null)
                {
                    Console.WriteLine($"⚠️ Permissão não encontrada para o ID de referência {idUsuario}");
                    return NotFound($"Permissão com ID de referência {idUsuario} não encontrada.");
                }

                return Ok(menu);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuRegraPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdUsuario={idUsuario}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Usuario/IdMenu/{idMenu}")]
        public async Task<IActionResult> ObterMenuUsuarioPorIdMenu(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var permissoes = await _servico.MenuUsuarioServicoBuscarPorIdMenuAsync(idMenu, cancellationToken);

                if (permissoes == null || !permissoes.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma permissão encontrada para o menu ID {idMenu}");
                    return NotFound($"Nenhuma permissão encontrada para o menu ID {idMenu}.");
                }

                return Ok(permissoes);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuUsuarioPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"IdUsuario={idMenu}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Usuario/Excluir/Referencia/{MenuId}/{UsuarioId}")]
        public async Task<IActionResult> ExcluirMenuUsuario(int MenuId, string UsuarioId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menusUsuario = await _servico.MenuUsuarioServicoBuscaPorIdReferenciaAsync(MenuId, UsuarioId, cancellationToken);

                if (menusUsuario == null)
                {
                    Console.WriteLine($"⚠️ Nenhum menu encontrado para o usuário ID: {UsuarioId}");
                    return NotFound($"Nenhum menu encontrado para o usuário ID {UsuarioId}.");
                }

                await _servico.MenuUsuarioServicoDeletarAsync(menusUsuario, cancellationToken);

                Console.WriteLine($"🗑️ Menus do usuário ID {UsuarioId} excluídos com sucesso.");
                return NoContent();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterMenuUsuarioPorIdReferencia",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={MenuId}, RegraId={UsuarioId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Menu com Referência à Integração

        [HttpPost]
        [Route("Integracao/Novo")]
        public async Task<IActionResult> MenuIntegracaoNovo([FromBody] List<MenuIntegracaoDto> integracoes, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (integracoes == null || !integracoes.Any())
                    return BadRequest(new { sucesso = false, mensagem = "A lista de integrações está vazia ou inválida." });

                await _servico.MenuIntegracaoServicoRegistrarAsync(integracoes, cancellationToken);

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
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "MenuIntegracaoNovo",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"request={integracoes}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdReferencia/{MenuId}/{IntegracaoId}")]
        public async Task<IActionResult> ObterIntegracaoPorIdReferencia(int MenuId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menu = await _servico.MenuIntegracaoServicoBuscaPorIdReferenciaAsync(MenuId, IntegracaoId, cancellationToken);

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
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterIntegracaoPorIdReferencia",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={MenuId}, IntegracaoId={IntegracaoId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdMenu/{idMenu}")]
        public async Task<IActionResult> ObterIntegracaoPorIdMenu(int idMenu, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var integracoes = await _servico.MenuIntegracaoServicoBuscarPorIdMenuAsync(idMenu, cancellationToken);

                if (integracoes == null || !integracoes.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para o menu ID {idMenu}");
                    return NotFound($"Nenhuma integração encontrada para o menu com ID {idMenu}.");
                }

                return Ok(integracoes);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterIntegracaoPorIdMenu",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={idMenu}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Integracao/Excluir/Referencia/{MenuId}/{IntegracaoId}")]
        public async Task<IActionResult> MenuIntegracaoExcluir(int MenuId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var menuIntegracao = await _servico.MenuIntegracaoServicoBuscaPorIdReferenciaAsync(MenuId, IntegracaoId, cancellationToken);

                if (menuIntegracao == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada para exclusão. ID: {MenuId}");
                    return NotFound($"Integração com ID {MenuId} não encontrada.");
                }

                await _servico.MenuIntegracaoServicoDeletarAsync(menuIntegracao, cancellationToken);

                Console.WriteLine($"🗑️ Integração ID {MenuId} excluída com sucesso.");
                return NoContent();
            }
            catch (KeyNotFoundException knf)
            {
                Console.WriteLine($"⚠️ {knf.Message}");
                return NotFound(knf.Message);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await _mongoLogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "MenuIntegracaoExcluir",
                    HttpMethod = HttpContext.Request.Method,
                    Path = HttpContext.Request.Path,
                    QueryString = HttpContext.Request.QueryString.ToString(),
                    UserName = User?.Identity?.Name,
                    ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    TraceId = HttpContext.TraceIdentifier,
                    ExceptionType = ex.GetType().ToString(),
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    Payload = $"MenuId={MenuId}, IntegracaoId={IntegracaoId}"
                });

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        #endregion
    }
}