using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Response;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Adobe_Hub
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "2.2-Integração do Adobe com o Hub: APIs do Histórico de Importações")]
    [Authorize]
    public class ImportHistoryController : ControllerBase
    {
        private readonly ImportHistoryService _service;
        private readonly IntegracaoAdobeHubLogErroRepository _LogErroRepo;

        public ImportHistoryController(ImportHistoryService service, IntegracaoAdobeHubLogErroRepository LogErroRepo)
        {
            _service = service;
            _LogErroRepo = LogErroRepo;
        }

        /// <summary>
        /// Lista todos os históricos de importação (com DTO seguro).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var historyList = await _service.GetAllImportHistoryAsync(cancellationToken);

                var dtoList = historyList.Select(h => new ImportHistoryDto
                {
                    Id = h.Id,
                    TemplateName = h.TemplateName,
                    FileName = h.FileName,
                    AttemptDate = h.AttemptDate,
                    Success = h.Success,
                    Pendencias = h.Pendencias
                }).ToList();

                return Ok(dtoList);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(GetAll), ex);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove um histórico específico pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("O ID fornecido é inválido.");
                }

                var deleted = await _service.DeleteImportHistoryByIdAsync(id, cancellationToken);

                if (!deleted)
                {
                    return NotFound($"Nenhum histórico encontrado com o ID: {id}");
                }

                return NoContent();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(Delete), ex, id);

                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        #region 🔧 Auxiliares

        private async Task LogErro(string endpoint, Exception ex, object? payload = null)
        {
            await _LogErroRepo.SaveAsync(new IntegracaoAdobeHubLogsErros
            {
                DataHoraUtc = DateTime.UtcNow,
                Endpoint = endpoint,
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
}
