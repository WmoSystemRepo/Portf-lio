using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Importar_Excel;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Templates.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Adobe_Hub
{
    [Route("api/Adobe/Planilhas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "2.1-Integração do Adobe com o Hub: APIs de Templates")]
    [Authorize]
    public class AdobeTemplatesController : ControllerBase
    {
        private readonly AdobePlanilhaService _adobePlanilhaService;
        private readonly MongoAdobeHubLogExclusaoTemplateRepository _mongoExclusaoTemplateRepo;
        private readonly IntegracaoAdobeHubLogErroRepository _LogErroRepo;

        public AdobeTemplatesController(
            AdobePlanilhaService adobePlanilhaService,
            MongoAdobeHubLogExclusaoTemplateRepository mongoExclusaoTemplateRepo,
            IntegracaoAdobeHubLogErroRepository mongoLogErroRepo)
        {
            _adobePlanilhaService = adobePlanilhaService ?? throw new ArgumentNullException(nameof(adobePlanilhaService));
            _mongoExclusaoTemplateRepo = mongoExclusaoTemplateRepo ?? throw new ArgumentNullException(nameof(mongoExclusaoTemplateRepo));
            _LogErroRepo = mongoLogErroRepo ?? throw new ArgumentNullException(nameof(mongoLogErroRepo));
        }

        #region 🔷 Templates

        /// <summary>
        /// Lista todos os templates de planilha armazenados no MongoDB.
        /// </summary>
        [HttpGet("Lista/Templantes")]
        public async Task<IActionResult> ListarTemplates(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var templates = await _adobePlanilhaService.ListarTemplatesAsync(cancellationToken);
                return Ok(templates);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(ListarTemplates), ex);
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria ou atualiza um novo template de planilha no MongoDB.
        /// </summary>
        [HttpPost("Novo/Templantes")]
        public async Task<IActionResult> NovoTemplente( [FromBody, Required] TemplantesMongoDto dto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _adobePlanilhaService.NovoTemplatesAsync(dto, cancellationToken);
                return Ok(new { message = $"Template '{dto.Nome}' criado com sucesso." });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(NovoTemplente), ex);
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove um template de planilha do MongoDB pelo ID.
        /// </summary>
        [HttpDelete("Excluir/Template")]
        public async Task<IActionResult> ExcluirTemplatePorId([FromBody] ExclusaoTemplateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto.RegistrosExcluidos == null || !dto.RegistrosExcluidos.Any())
                    return BadRequest("Nenhum registro informado para exclusão.");

                cancellationToken.ThrowIfCancellationRequested();

                dto.Usuario ??= User?.Identity?.Name
                    ?? User?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value
                    ?? User?.Claims?.FirstOrDefault(c => c.Type == "unique_name")?.Value
                    ?? "usuario@desconhecido.com";

                var id = dto.RegistrosExcluidos.First().Id;

                bool removido = await _adobePlanilhaService.RemoverTemplatePorIdAsync(id, cancellationToken);

                if (!removido)
                    return NotFound($"Template com nome '{id}' não encontrado.");

                var registrosExcluidosMongo = dto.RegistrosExcluidos.Select(p => new RegistroExcluidoTemplateMongo
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    QtdColunas = p.QtdColunas,
                    Colunas = p.Colunas,
                    LinhaCabecalho = p.LinhaCabecalho,
                    ColunaInicial = p.ColunaInicial,
                    ArquivoBase = p.ArquivoBase,
                    ObservacaoDescricao = p.ObservacaoDescricao,
                    ColunasObrigatorias = p.ColunasObrigatorias,
                    DataCriacao = p.DataCriacao,
                    DataEdicao = p.DataEdicao,
                    Template = p.TipoTemplate
                }).ToList();

                var logExclusao = new IntegracaoAdobeHubTemplateExclusaoLogMongo
                {
                    DataHora = dto.DataHora,
                    Usuario = dto.Usuario,
                    Justificativa = dto.Justificativa,
                    RegistrosExcluidos = registrosExcluidosMongo,
                    Endpoint = nameof(ExcluirTemplatePorId),
                    MigradoParaSql = false
                };

                await _mongoExclusaoTemplateRepo.SaveAsync(logExclusao, cancellationToken);

                return Ok(new { message = $"Template com ID '{id}' removido com sucesso." });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                var id = dto.RegistrosExcluidos?.FirstOrDefault()?.Id ?? "(sem-id)";
                await LogErro(nameof(ExcluirTemplatePorId), ex, id);
                return StatusCode(500, $"Erro ao remover template: {ex.Message}");
            }
        }

        #endregion

        #region 🔧 Auxiliares

        private async Task LogErro(string endpoint, Exception ex, object? payload = null)
        {
            await _LogErroRepo.SaveAsync(new IntegracaoAdobeHubLogsErros
            {
                DataHoraUtc = DateTime.UtcNow,
                Endpoint = nameof(endpoint),
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