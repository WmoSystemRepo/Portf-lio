using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Exceptions.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.SppBimerInvoce
{
    public class MonitorSppBimerInvoceRepositorio : IMonitorSppBimerInvoceRepositorio
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MonitorSppBimerInvoceRepositorio(
            Context context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<IntegracaoSppBimerInvoce>> BuscarFiltrosAsync(
            string? status, DateTime? dataInicio, DateTime? dataFim, CancellationToken ct)
        {
            try
            {
                var query = _context.IntegracaoSppBimerInvoce
                    .AsNoTracking()
                    .Include(i => i.Resumos)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(i => i.StatusIntegracaoAlterData == status);

                if (dataInicio.HasValue)
                {
                    var di = dataInicio.Value.Date;
                    query = query.Where(i => i.DataEmissao >= di);
                }

                if (dataFim.HasValue)
                {
                    // inclui o dia inteiro de dataFim (até 23:59:59)
                    var dfExclusive = dataFim.Value.Date.AddDays(1);
                    query = query.Where(i => i.DataEmissao < dfExclusive);
                }

                return await query.ToListAsync(ct);
            }
            catch (Exception ex)
            {
                var details = $"status={status ?? "<null>"}, " +
                              $"dataInicio={(dataInicio?.ToString("yyyy-MM-dd") ?? "<null>")}, " +
                              $"dataFim={(dataFim?.ToString("yyyy-MM-dd") ?? "<null>")}";
                throw new DataAccessException(
                    $"{nameof(MonitorSppBimerInvoceRepositorio)}.{nameof(BuscarFiltrosAsync)}",
                    details,
                    ex);
            }
        }

        public async Task<IntegracaoSppBimerInvoce?> ObterPorIdAsync(int id, CancellationToken ct)
        {
            try
            {
                return await _context.IntegracaoSppBimerInvoce
                    .AsNoTracking()
                    .Include(i => i.Resumos)
                    .FirstOrDefaultAsync(i => i.CdEmpresa == id, ct);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(
                    $"{nameof(MonitorSppBimerInvoceRepositorio)}.{nameof(ObterPorIdAsync)}",
                    $"id={id}",
                    ex);
            }
        }

        public async Task AtualizarAsync(IntegracaoSppBimerInvoce entidade, CancellationToken ct)
        {
            try
            {
                _context.IntegracaoSppBimerInvoce.Update(entidade);
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                var details = $"entityId={entidade?.CdEmpresa}";
                throw new DataAccessException(
                    $"{nameof(MonitorSppBimerInvoceRepositorio)}.{nameof(AtualizarAsync)}",
                    details,
                    ex);
            }
        }

        public async Task<IEnumerable<IntegracaoSppBimerInvoce>> ListarTodosAsync(CancellationToken ct)
        {
            try
            {
                return await _context.IntegracaoSppBimerInvoce
                    .AsNoTracking()
                    .Include(i => i.Resumos)
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(
                    $"{nameof(MonitorSppBimerInvoceRepositorio)}.{nameof(ListarTodosAsync)}",
                    details: null,
                    ex);
            }
        }

        public async Task<string> ReprocessarAsync(ReprocessarBimerRequestDto dto, CancellationToken ct)
        {
            try
            {
                var fabricante = dto.Fabricante?.Trim();
                var pedido = dto.Pedido?.Trim();
                var estoque = dto.Estoque?.Trim();

                if (string.IsNullOrWhiteSpace(fabricante) ||
                    string.IsNullOrWhiteSpace(pedido) ||
                    string.IsNullOrWhiteSpace(estoque))
                {
                    throw new ArgumentException("Dados obrigatórios não informados.");
                }

                var baseUrl = _configuration["SistemaSpp:ReprocessamentoBaseUrl"];
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new DataAccessException(
                        nameof(ReprocessarAsync),
                        "URL base de reprocessamento não configurada.",
                        inner: new InvalidOperationException("Missing SistemaSpp:ReprocessamentoBaseUrl"));
                }

                var url = $"{baseUrl}/Comercial/comERPGeraNotaSaidaReprocessa.cfm" +
                          $"?pedido={pedido}&fabricante={fabricante}&estoque={estoque}";

                var response = await _httpClient.GetAsync(url, ct);

                if (!response.IsSuccessStatusCode)
                {
                    throw new DataAccessException(
                        nameof(ReprocessarAsync),
                        $"Erro ao reprocessar. HTTP {(int)response.StatusCode}",
                        inner: new HttpRequestException(response.ReasonPhrase));
                }

                return "Reprocessamento iniciado com sucesso.";
            }
            catch (Exception ex)
            {
                var details = $"pedido={dto?.Pedido}; fabricante={dto?.Fabricante}; estoque={dto?.Estoque}";
                throw new DataAccessException(
                    $"{nameof(MonitorSppBimerInvoceRepositorio)}.{nameof(ReprocessarAsync)}",
                    details,
                    ex);
            }
        }
    }
}
