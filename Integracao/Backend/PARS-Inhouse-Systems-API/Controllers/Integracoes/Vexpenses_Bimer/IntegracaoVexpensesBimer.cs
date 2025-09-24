using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.VexpessesBimerInvoce;
using PARS.Inhouse.Systems.Domain.DTOs.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense.ExclussãoPendecias;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ErrosIntegracoes;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer.ExclusaoPendentes;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq.Expressions;

namespace PARS_Inhouse_Systems_API.Controllers.Integracoes.Vexpense_Bimer
{
    [ApiController]
    [Route("api/Integracao")]
    [ApiExplorerSettings(GroupName = "1-Integração Vexpense com Bimer")]
    [Authorize]
    public class IntegracaoVexpensesBimer : Controller
    {
        private readonly IIntegracaoBimerService _integracaoBimerService;
        private readonly IVExpensesService _vexpesssesService;
        private readonly IConfiguration _configuration;
        private readonly IntegracaoVexpensesBimmerLogErroRepository _LogErroRepo;
        private readonly MongoVexpensesBimmerExclusaoLogRepository _mongoExclusaoRepo;

        public IntegracaoVexpensesBimer(IIntegracaoBimerService integracaoBimerService,
                                        IConfiguration configuration,
                                        IVExpensesService vExpenses,
                                        MongoVexpensesBimmerExclusaoLogRepository mongoExclusaoRepo,
                                        IntegracaoVexpensesBimmerLogErroRepository LogErroRepo)
        {
            _integracaoBimerService = integracaoBimerService;
            _configuration = configuration;
            _LogErroRepo = LogErroRepo;
            _vexpesssesService = vExpenses;
            _mongoExclusaoRepo = mongoExclusaoRepo;

        }

        [HttpPost("Inserir/Titulos/Manual")]
        public async Task<IActionResult> InserirTituloManual([FromBody] int[] ids, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var token = await ObterTokenDeAutenticacaoBimmerAsync(cancellationToken);
                if (token == null)
                    return BadRequest("Configurações de autenticação estão incompletas ou ausentes.");

                var result = await _integracaoBimerService.ProcessarEnvioTitulosParaBimmerAsync(token.access_token, true, ids, cancellationToken);

                var resposta = result.Select(x => new
                {
                    Id = x.Item1,
                    Descricao = x.Item2,
                    Integrado = x.Item3,
                    Mensagem = x.Item3 ? "Integrado com sucesso" : "Falha na integração"
                });

                return Ok(resposta);
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
                    Endpoint = "InserirTituloEmMassa",
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
                    Payload = $"ids={ids}"
                });

                return StatusCode(500, $"Erro ao processar a integração: {ex.Message}");
            }
        }

        [HttpPost("Inserir/Titulos/EmMassa")]
        public async Task<IActionResult> InserirTituloEmMassa(CancellationToken cancellationToken)
        {
            try
            {
                var token = await ObterTokenDeAutenticacaoBimmerAsync(cancellationToken);
                if (token == null)
                    return BadRequest("Configurações de autenticação estão incompletas ou ausentes.");

                var result = await _integracaoBimerService.ProcessarEnvioTitulosParaBimmerAsync(
                    token.access_token,
                    false,
                    cancellationToken: cancellationToken
                );

                return Ok(result.Select(x => new
                {
                    Id = x.Item1,
                    Descricao = x.Item2,
                    Integrado = x.Item3,
                    Mensagem = x.Item3 ? "Integrado com sucesso" : "Falha na integração"
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
                    Endpoint = "InserirTituloEmMassa",
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
                    Payload = string.Empty
                });

                return StatusCode(500, $"Erro ao processar a integração: {ex.Message}");
            }
        }

        [HttpGet("Pendencias")]
        public async Task<IActionResult> ObterPendencias([FromQuery] int pageNumber = 1,
                                                         [FromQuery] int pageSize = 10,
                                                         [FromQuery] string? search = null,
                                                         CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _integracaoBimerService.ObterPendenciasDeIntegracaoAsync(
                    pageNumber,
                    pageSize,
                    search,
                    cancellationToken
                );

                return Ok(result);
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
                    Endpoint = "ObterPendencias",
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
                    Payload = $"pageNumber={pageNumber}, pageSize={pageSize}, search={search}"
                });

                return StatusCode(500, $"Erro ao buscar Pendências: {ex.Message}");
            }
        }

        [HttpGet("Lista/Erros")]
        public async Task<PaginatedResultDto<IntegracaoVexpenssesBimmerLogErros>> ObterIntegracaoErros(int pageNumber, int pageSize, string? search)
        {
            try
            {
                Expression<Func<IntegracaoVexpenssesBimmerLogErros, bool>> filter = x => true;


                if (!string.IsNullOrWhiteSpace(search))
                {
                    filter = x =>
                    EF.Functions.Like(x.Endpoint!, $"%{search}%") ||
                    EF.Functions.Like(x.Message!, $"%{search}%") ||
                    EF.Functions.Like(x.Payload!, $"%{search}%");
                }


                var total = await _LogErroRepo.CountDocumentsAsync(filter);
                var items = await _LogErroRepo.GetPaginatedAsync(pageNumber, pageSize, search);


                return new PaginatedResultDto<IntegracaoVexpenssesBimmerLogErros>
                {
                    Items = items,
                    TotalCount = total
                };
            }
            catch (Exception ex)
            {
                try
                {
                    await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                    {
                        DataHoraUtc = DateTime.UtcNow,
                        Endpoint = "ObterIntegracaoErros",
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
                        Payload = $"pageNumber={pageNumber}, pageSize={pageSize}, search={search}"
                    });
                }
                catch (Exception logEx)
                {
                    Console.WriteLine("Erro ao tentar salvar log no SQL Server: " + logEx);
                }


                throw new ApplicationException("Erro ao consultar erros de integração no SQL Server.", ex);
            }
        }

        [HttpGet("Sucesso")]
        public async Task<IActionResult> ObterIntegracaoSucesso([FromQuery] int pageNumber = 1,
                                                                [FromQuery] int pageSize = 10,
                                                                [FromQuery] string? status = null,
                                                                [FromQuery] string? search = null,
                                                                CancellationToken cancellationToken = default)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest(new { Message = "Os parâmetros pageNumber e pageSize devem ser maiores que zero." });
                }

                var result = await _integracaoBimerService
                    .ObterInsercoesBemSucedidasDeIntegracaoComPaginamentoAsync(pageNumber, pageSize, search, cancellationToken);

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Requisição cancelada pelo cliente.");
            }
            catch (Exception ex)
            {
                try
                {
                    await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                    {
                        DataHoraUtc = DateTime.UtcNow,
                        Endpoint = "ObterIntegracaoSucesso",
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
                        Payload = $"pageNumber={pageNumber}, pageSize={pageSize}, search={search}"
                    });
                }
                catch (Exception logEx)
                {
                    Console.WriteLine("Erro ao tentar salvar log no Mongo: " + logEx);
                }

                return StatusCode(500, $"Erro ao buscar inserções com sucesso integração bimmer: {ex.Message}");
            }
        }

        [HttpGet("Vexpensses/Bimer/Dashboard")]
        public async Task<IActionResult> ObterDashboardVexpenses(CancellationToken cancellationToken)
        {
            try
            {
                var dto = new DashboardVexpensesBimerDto();

                var pendencias = await _integracaoBimerService.ObterPendenciasDeIntegracaoAsync(1, int.MaxValue, null, cancellationToken);
                var erros = await _LogErroRepo.GetAllAsync();
                var sucesso = await _integracaoBimerService.ObterInsercoesBemSucedidasDeIntegracaoComPaginamentoAsync(1, int.MaxValue, null, cancellationToken);
                var pendenciasResolvidas = await _mongoExclusaoRepo.GetAllAsync(cancellationToken);

                dto.Totais.TotalPendencias = pendencias.Count();
                dto.Totais.TotalErros = erros.Count();
                dto.Totais.TotalSucesso = sucesso.Count();

                var contagens = await _vexpesssesService.ObterContagensRelatoriosAsync(cancellationToken);

                dto.Totais.TotalPago = contagens.TotalPago;
                dto.Totais.TotalAprovados = contagens.TotalAprovados;
                dto.Totais.TotalAbertos = contagens.TotalAbertos;
                dto.Totais.TotalEnviados = contagens.TotalEnviados;
                dto.Totais.TotalReabertos = contagens.TotalReabertos;
                dto.Totais.TotalReprovados = contagens.TotalReprovados;

                dto.Totais.TotalGeral = dto.Totais.TotalPendencias +
                                        dto.Totais.TotalErros +
                                        dto.Totais.TotalSucesso +
                                        dto.Totais.TotalPago +
                                        dto.Totais.TotalAprovados +
                                        dto.Totais.TotalAbertos +
                                        dto.Totais.TotalEnviados +
                                        dto.Totais.TotalReabertos +
                                        dto.Totais.TotalReprovados;


                var anoAtual = DateTime.UtcNow.Year;
                var meses = Enumerable.Range(1, 7).ToList();

                dto.GraficoMensal.Labels = meses
                    .Select(m => new DateTime(anoAtual, m, 1).ToString("MMM", new CultureInfo("pt-BR")))
                    .ToList();

                dto.GraficoMensal.Pendencias = meses.Select(m =>
                    pendencias.Count(p => p.DataCadastro.HasValue && p.DataCadastro.Value.Month == m && p.DataCadastro.Value.Year == anoAtual)
                ).ToList();

                dto.GraficoMensal.PendenciasResolvidas = meses.Select(m =>
                    pendenciasResolvidas.Count(e => e.DataHora.Month == m && e.DataHora.Year == anoAtual)
                ).ToList();

                dto.GraficoMensal.Erros = meses.Select(m =>
                    erros.Count(e => e.DataHoraUtc.Month == m && e.DataHoraUtc.Year == anoAtual)
                ).ToList();

                dto.GraficoMensal.Sucesso = meses.Select(m =>
                    sucesso.Count(s => s.DataCadastro.HasValue && s.DataCadastro.Value.Month == m && s.DataCadastro.Value.Year == anoAtual)
                ).ToList();

                var errosMongo = await _LogErroRepo.GetAllAsync(cancellationToken);

                var errosPorTipo = errosMongo
                    .GroupBy(e => ExtrairTipoExcecao(e.Message))
                    .Select(g => new { Tipo = g.Key, Quantidade = g.Count() })
                    .ToList();

                dto.GraficoPizzaErros.Labels = errosPorTipo.Select(e => e.Tipo).ToList();
                dto.GraficoPizzaErros.Valores = errosPorTipo.Select(e => e.Quantidade).ToList();

                var pendenciasIntegracao = await _integracaoBimerService.ObterPendenciasDeIntegracaoAsync(1, int.MaxValue, null, cancellationToken);

                var pendenciasPorStatus = pendenciasIntegracao
                    .GroupBy(p => ExtrairStatusPendencia(p.Observacao))
                    .Select(g => new { Tipo = g.Key, Quantidade = g.Count() })
                    .ToList();


                dto.GraficoPizzaPendencias.Labels = pendenciasPorStatus.Select(p => p.Tipo).ToList();
                dto.GraficoPizzaPendencias.Valores = pendenciasPorStatus.Select(p => p.Quantidade).ToList();

                var pendenciasExcluidas = await _mongoExclusaoRepo.GetAllAsync(cancellationToken);

                var pendenciasPorUsuario = pendenciasExcluidas
                    .GroupBy(p => p.Usuario)
                    .Select(g => new { Usuario = g.Key, Quantidade = g.Count() })
                    .ToList();

                dto.GraficoPizzaPendenciasExcluidasPorUsario = new GraficoPizzaDto
                {
                    Labels = pendenciasPorUsuario.Select(p => p.Usuario).ToList(),
                    Valores = pendenciasPorUsuario.Select(p => p.Quantidade).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ObterDashboardVexpenses",
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
                    Payload = string.Empty
                });

                return StatusCode(500, $"Erro ao montar o dashboard: {ex.Message}");
            }
        }

        [HttpDelete("Excluir/Pendencias")]
        public async Task<IActionResult> ExcluirPendencias([FromBody] ExclusaoPendenciasDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto.RegistrosExcluidos == null || !dto.RegistrosExcluidos.Any())
                    return BadRequest("Nenhum registro informado para exclusão.");

                dto.Usuario ??= User?.Identity?.Name
                    ?? User?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value
                    ?? User?.Claims?.FirstOrDefault(c => c.Type == "unique_name")?.Value
                    ?? "usuario@desconhecido.com";

                var ids = dto.RegistrosExcluidos.Select(p => p.IdResponse).ToArray();

                await _integracaoBimerService.ExcluirPendenciasAsync(ids, cancellationToken);

                var registrosExcluidosMongo = dto.RegistrosExcluidos.Select(p => new RegistroExcluidoMongo
                {
                    IdResponse = p.IdResponse,
                    UserId = p.UserId,
                    Descricao = p.Descricao,
                    Valor = p.Valor,
                    DataCadastro = p.DataCadastro,
                    Observacao = p.Observacao,
                    Response = p.Response
                }).ToList();

                var logExclusao = new IntegracaoVexpensesBimmerExclusaoLogMongo
                {
                    DataHora = dto.DataHora,
                    Usuario = dto.Usuario,
                    Justificativa = dto.Justificativa,
                    RegistrosExcluidos = registrosExcluidosMongo,
                    Endpoint = nameof(ExcluirPendencias),
                    MigradoParaSql = true
                };

                await _mongoExclusaoRepo.SaveAsync(logExclusao, cancellationToken);

                return Ok(new
                {
                    Mensagem = "Registros excluídos com sucesso.",
                    Quantidade = ids.Length
                });
            }
            catch (Exception ex)
            {
                await _LogErroRepo.SaveAsync(new IntegracaoVexpenssesBimmerLogErros
                {
                    DataHoraUtc = DateTime.UtcNow,
                    Endpoint = "ExcluirPendencias",
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
                    Payload = $"request={dto}"
                });

                return StatusCode(500, $"Erro ao excluir pendências: {ex.Message}");
            }
        }

        private async Task<AuthResponseDto?> ObterTokenDeAutenticacaoBimmerAsync(CancellationToken cancellationToken)
        {
            try
            {
                var clientId = _configuration["BimmerAuth:client_id"];
                var clientSecret = _configuration["BimmerAuth:client_secret"];
                var grantType = _configuration["BimmerAuth:grant_type"];
                var username = _configuration["BimmerAuth:username"];
                var password = _configuration["BimmerAuth:password"];
                var nonce = _configuration["BimmerAuth:nonce"];

                if (string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(grantType) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(nonce))
                {
                    return null;
                }

                return await _integracaoBimerService.AutenticarNoBimmerAsync(new AuthRequestDto
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    GrantType = grantType,
                    Username = username,
                    Password = password,
                    Nonce = nonce
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string ExtrairTipoExcecao(string? erro)
        {
            if (string.IsNullOrWhiteSpace(erro)) return "Desconhecido";

            var primeiraLinha = erro.Split('\n').FirstOrDefault() ?? erro;
            var tipo = primeiraLinha.Split(':').FirstOrDefault();
            return tipo?.Trim() ?? "Desconhecido";
        }

        private string ExtrairStatusPendencia(string? pendencia)
        {
            if (string.IsNullOrWhiteSpace(pendencia)) return "Desconhecido";

            var primeiraLinha = pendencia.Split('\n').FirstOrDefault() ?? pendencia;
            var status = primeiraLinha.Split(':').FirstOrDefault();
            return status?.Trim() ?? "Desconhecido";
        }
    }
}