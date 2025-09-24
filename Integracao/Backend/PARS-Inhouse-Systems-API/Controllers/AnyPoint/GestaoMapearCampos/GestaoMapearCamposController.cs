using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.AnyPoint.GestaoMapearCampos
{
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: Gestao Mapear Campos")]
    [Route("api/GestaoMapearCampos")]
    [ApiController]
    [Authorize]
    public class GestaoMapearCamposController : ControllerBase
    {
        private readonly IGestaoMepeamentoComposService _service;
        private readonly IIntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;

        public GestaoMapearCamposController(IGestaoMepeamentoComposService service, IIntegracaoVexpensesBimmerLogErroRepository LogErroRepo)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _LogErroRepo = LogErroRepo ?? throw new ArgumentNullException(nameof(LogErroRepo));
        }

        #region Controladores do Mepeamento Campos
        [HttpGet("Lista")]
        public async Task<IActionResult> ListarMepeamentoCampos(CancellationToken cancellationToken)

        {
            try
            {
                var result = await _service.ListarMapeamentoCamposAsync(cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(ListarMepeamentoCampos),
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

                return StatusCode(500, $"Erro ao buscar registros: {ex.Message}");
            }
        }

        [HttpGet("Id/{id}")]
        public async Task<IActionResult> ObterPorId(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            try
            {
                var result = await _service.ObterPorIdAsync(id, cancellationToken);
                if (result == null)
                    return NotFound($"Registro com ID {id} não encontrado.");

                return Ok(result);
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
                    Payload = $"ID={id}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao buscar registro: {ex.Message}");
            }
        }

        [HttpPost("Novo")]
        public async Task<IActionResult> Novo([FromBody] AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                return BadRequest("Os dados não podem ser nulos.");

            try
            {
                await _service.NovoRegistroAsync(entity, cancellationToken);
                return CreatedAtAction(nameof(ObterPorId), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Novo),
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
                    Payload = $"{entity}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao criar registro: {ex.Message}");
            }
        }

        [HttpPut("Editar/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            if (entity == null || id <= 0 || id != entity.Id)
                return BadRequest("Dados inválidos ou ID inconsistente.");

            try
            {
                await _service.EditarRegistroAsync(entity, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Registro com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Editar),
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
                    Payload = $"ID={id}, {entity}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao atualizar registro: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/{id:int}")]
        public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            try
            {
                await _service.ExcluirRegistroAsync(id, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Registro com ID {id} não encontrado.");
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(Editar),
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
                    Payload = $"ID={id}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro ao excluir registro: {ex.Message}");
            }
        }
        #endregion

        #region Controladores de Menu com Referência à Integração

        [HttpPost("Integracao/Novo")]
        public async Task<IActionResult> MapeamentoIntegracaoNovo([FromBody] List<MapeamentoIntegracaoDto> integracoes, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (integracoes == null || !integracoes.Any())
                    return BadRequest(new { sucesso = false, mensagem = "A lista de integrações está vazia ou inválida." });

                await _service.MapeamentoIntegracaoServicoRegistrarAsync(integracoes, cancellationToken);

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
                    Endpoint = nameof(MapeamentoIntegracaoNovo),
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
                    Payload = $"ID={integracoes}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdReferencia/{MapeamentoId}/{IntegracaoId}")]
        public async Task<IActionResult> ObterMapeamentoIntegracaoPorIdReferencia(int MapeamentoId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var mapeamento = await _service.MapeamentoIntegracaoServicoBuscaPorIdReferenciaAsync(MapeamentoId, IntegracaoId, cancellationToken);

                if (mapeamento == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada com o ID de referência {IntegracaoId}");
                    return NotFound($"Integração com ID de referência {IntegracaoId} não encontrada.");
                }

                return Ok(mapeamento);
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
                    Endpoint = nameof(ObterMapeamentoIntegracaoPorIdReferencia),
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
                    Payload = $"MapeamentoId={MapeamentoId}, IntegraçãoId={IntegracaoId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpGet("Integracao/IdMapeamento/{idMapeamento}")]
        public async Task<IActionResult> ObterIntegracaoPorIdMapeamento(int idMapeamento, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var integracoes = await _service.MapeamentoIntegracaoServicoBuscarPorIdMapeamentoAsync(idMapeamento, cancellationToken);

                if (integracoes == null || !integracoes.Any())
                {
                    Console.WriteLine($"⚠️ Nenhuma integração encontrada para o mapeamento ID {idMapeamento}");
                    return NotFound($"Nenhuma integração encontrada para o mapeamento com ID {idMapeamento}.");
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
                    Endpoint = nameof(ObterMapeamentoIntegracaoPorIdReferencia),
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
                    Payload = $"MapeamentoId={idMapeamento}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        [HttpDelete("Integracao/Excluir/Referencia/{MapeamentoId}/{IntegracaoId}")]
        public async Task<IActionResult> MapeamentoIntegracaoExcluir(int MapeamentoId, int IntegracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var mapeamentoIntegracao = await _service.MapeamentoIntegracaoServicoBuscaPorIdReferenciaAsync(MapeamentoId, IntegracaoId, cancellationToken);

                if (mapeamentoIntegracao == null)
                {
                    Console.WriteLine($"⚠️ Integração não encontrada para exclusão. ID: {IntegracaoId}");
                    return NotFound($"Integração com ID {IntegracaoId} não encontrada.");
                }

                await _service.MapeamentoIntegracaoServicoDeletarAsync(mapeamentoIntegracao, cancellationToken);

                Console.WriteLine($"🗑️ Integração ID {IntegracaoId} excluída com sucesso.");
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
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = nameof(MapeamentoIntegracaoExcluir),
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
                    Payload = $"MapeamentoId={MapeamentoId}, IntegraçãoId={IntegracaoId}"
                }, CancellationToken.None);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }
        #endregion
    }
}