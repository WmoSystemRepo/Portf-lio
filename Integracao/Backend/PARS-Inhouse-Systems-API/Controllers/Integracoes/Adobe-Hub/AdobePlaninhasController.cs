using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Importar_Excel;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.AdobeHub.LogsErros;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Shared.Exceptions.Integracao.AdobeHub;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Adobe_Hub
{
    [Route("api/Adobe/Planilhas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "2.1-Integração Adobe Hub: Gestão de Templates de Importação")]
    [Authorize]
    public class AdobePlaninhasController : ControllerBase
    {
        private readonly IntegracaoAdobeHubLogErroRepository _LogErroRepo;
        private readonly IImportHistoryRepository _historyRepository;
        private readonly AdobePlanilhaService _adobePlanilhaService;
        private readonly MongoAdobeHubLogExclusaoPlanilhaRepository _mongoExclusaoPlanilhaRepo;
        private readonly IPrecoRevendaService _precoRevendaService;

        public AdobePlaninhasController(
            IImportHistoryRepository historyRepository,
            IntegracaoAdobeHubLogErroRepository LogErroRepo,
            AdobePlanilhaService adobePlanilhaService,
            MongoAdobeHubLogExclusaoPlanilhaRepository mongoExclusaoplanilhaRepo,
            IPrecoRevendaService precoRevendaService)
        {
            _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
            _LogErroRepo = LogErroRepo ?? throw new ArgumentNullException(nameof(LogErroRepo));
            _adobePlanilhaService = adobePlanilhaService ?? throw new ArgumentNullException(nameof(adobePlanilhaService));
            _mongoExclusaoPlanilhaRepo = mongoExclusaoplanilhaRepo ?? throw new ArgumentNullException(nameof(mongoExclusaoplanilhaRepo));
            _precoRevendaService = precoRevendaService ?? throw new ArgumentNullException(nameof(precoRevendaService));
        }

        #region 📥 Planilhas

        /// <summary>
        /// Lista todas as planilhas importadas já armazenadas no MongoDB.
        /// </summary>
        [HttpGet("Lista/Planilhas/Importadas")]
        public async Task<IActionResult> ListarPlanilhasImportadas(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var planilhas = await _adobePlanilhaService.ListaPlaninhasImportadasAsync(cancellationToken);
                return Ok(planilhas);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(ListarPlanilhasImportadas), ex);
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Importa uma nova planilha para o MongoDB após validação de colunas com base no template esperado.
        /// </summary>
        [HttpPost("Importar/Novo/Excel")]
        public async Task<IActionResult> NovaPlaninha([FromBody, Required] PlaninhasImportadosDto dto, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _adobePlanilhaService.SalvarPlanilhaImportadaAsync(dto, cancellationToken);
                return Ok(new { message = $"Planilha '{dto.NomeArquivo}' criada com sucesso." });
            }
            catch (ValidacaoColunasException colEx)
            {
                return BadRequest(new
                {
                    erro = colEx.Message,
                    faltando = colEx.Detalhes.ColunasFaltando,
                    extras = colEx.Detalhes.ColunasExtras
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(NovaPlaninha), ex, dto);
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca os dados de uma planilha específica por ID.
        /// Aplica o cálculo de preço de revenda se o campo "Partner Price" estiver presente.
        /// </summary>
        [HttpGet("Obter/Id/{id}")]
        public async Task<IActionResult> BuscarPorId([FromRoute] string id, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("ID inválido ou ausente na rota.");
                }

                cancellationToken.ThrowIfCancellationRequested();

                var planilha = await _adobePlanilhaService.BuscarPlanilhaPorIdAsync(id, cancellationToken);
                if (planilha == null)
                {
                    return NotFound($"Planilha com ID '{id}' não encontrada.");
                }

                //if (planilha.Dados != null && planilha.Dados.Any())
                //{
                //    var dadosConvertidos = planilha.Dados
                //        .Select(dict => dict.ToDictionary(kvp => kvp.Key, kvp => (object?)kvp.Value))
                //        .ToList();

                //    //await _precoRevendaService.AplicarPrecoRevendaAsync(dadosConvertidos, cancellationToken);
                //}

                return Ok(planilha);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(BuscarPorId), ex, id);
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove uma planilha armazenada no MongoDB pelo ID.
        /// </summary>
        [HttpDelete("Excluir/Planilha")]
        public async Task<IActionResult> ExcluirPlanilhaPorId(
            [FromBody] ExclusaoPlanilhaDto dto,
            CancellationToken cancellationToken)
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

                bool removido = await _adobePlanilhaService.RemoverPlanilhaPorIdAsync(id, cancellationToken);

                if (!removido)
                    return NotFound($"Template com nome '{id}' não encontrado.");

                var registrosExcluidosMongo = dto.RegistrosExcluidos.Select(static p => new RegistroExcluidoPlanilhaMongo
                {
                    Id = p.Id,
                    NomeArquivo = p.NomeArquivo,
                    DataUpload = p.DataUpload,
                    Dados = p.Dados,
                    TipoTemplante = p.Template?.TipoTemplate
                }).ToList();

                var logExclusao = new IntegracaoAdobeHubPlanilhaExclusaoLogMongo
                {
                    DataHora = dto.DataHora,
                    Usuario = dto.Usuario,
                    Justificativa = dto.Justificativa,
                    RegistrosExcluidos = registrosExcluidosMongo,
                    Endpoint = nameof(ExcluirPlanilhaPorId),
                    MigradoParaSql = false
                };

                await _mongoExclusaoPlanilhaRepo.SaveAsync(logExclusao, cancellationToken);

                return Ok(new { message = $"Template '{id}' removido com sucesso." });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                var id = dto.RegistrosExcluidos.First().Id;
                await LogErro(nameof(ExcluirPlanilhaPorId), ex, id);
                return StatusCode(500, $"Erro ao remover template: {ex.Message}");
            }
        }

        [HttpPost("Planilha/{id}/Produtos/Salvar")]
        public async Task<IActionResult> SalvarProduto([FromRoute] string id, [FromBody, Required] SalvarProdutoDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("ID da planilha é obrigatório.");
                }

                if (dto == null || dto.Produto == null || dto.Produto.Count == 0)
                {
                    return BadRequest("Os dados do produto não podem estar vazios.");
                }

                cancellationToken.ThrowIfCancellationRequested();

                var status = await _adobePlanilhaService.SalvarProdutoAsync(id, dto, cancellationToken);

                return Ok(new { status });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                await LogErro(nameof(SalvarProduto), ex, new { id, dto?.Produto });
                return StatusCode(500, $"Erro no processo: {ex.Message}");
            }
        }

        #endregion

        #region 🔧 Auxiliares (inalterados)

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

        private static string NormalizarNomeColuna(string nome)
        {
            return nome?
                .Trim()
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("_", "")
                ?? string.Empty;
        }

        #endregion
    }
}
