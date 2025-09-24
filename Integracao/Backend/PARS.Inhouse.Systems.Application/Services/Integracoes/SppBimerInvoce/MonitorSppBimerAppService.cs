using PARS.Inhouse.Systems.Application.Exceptions.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoice;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.SppBimerInvoice
{
    public class MonitorSppBimerAppService : IMonitorSppBimerAppService
    {
        private readonly IMonitorSppBimerInvoceRepositorio _repository;

        public MonitorSppBimerAppService(IMonitorSppBimerInvoceRepositorio repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MonitoramentoSppBimerInvoceDto>> ListarTodosAsync(CancellationToken ct)
        {
            try
            {
                var entidades = await _repository.ListarTodosAsync(ct);

                return entidades.Select(static entity =>
                {
                    var fabricante = entity.NomeDoFabricante ?? string.Empty;
                    var fabricanteLower = fabricante.ToLowerInvariant();

                    var invoice = string.IsNullOrWhiteSpace(entity.NumeroDaNFSe)
                                  && fabricanteLower.Contains("red hot")
                                  ? (entity.NumeroDocumento ?? string.Empty)
                                  : (entity.NumeroDaNFSe ?? string.Empty);

                    var dataEmissao = entity.DataEmissao == default
                        ? DateTime.MinValue
                        : entity.DataEmissao;

                    var valorInvoice = (entity.Resumos?.Sum(r => r?.ValorPagamentoTotal ?? 0m)) ?? 0m;

                    return new MonitoramentoSppBimerInvoceDto
                    {
                        Id = entity.CdEmpresa,
                        NumeroAlterData = entity.NumeroDocumento ?? string.Empty,
                        PedidoSpp = invoice,
                        DataEmissao = dataEmissao,
                        ValorInvoice = valorInvoice,
                        StatusIntegracao = entity.StatusIntegracaoAlterData ?? "N",
                        ObservacaoErro = entity.StatusIntegracaoAlterDataObs ?? string.Empty,
                        Fabricante = fabricante,
                        Estoque = entity.Estoque ?? string.Empty,
                        FabricanteId = entity.FabricanteId?.ToString()
                    };
                });
            }
            catch (Exception ex)
            {
                throw new SppBimerInvoceServiceException(
                    $"{nameof(MonitorSppBimerAppService)}.{nameof(ListarTodosAsync)}",
                    details: null,
                    inner: ex);
            }
        }

        public async Task<IEnumerable<MonitoramentoSppBimerInvoceDto>> ObterMonitoramentosAsync(
            string? status, DateTime? dataInicio, DateTime? dataFim, CancellationToken ct)
        {
            try
            {
                var entidades =
                    (!string.IsNullOrWhiteSpace(status) || dataInicio.HasValue || dataFim.HasValue)
                        ? await _repository.BuscarFiltrosAsync(status, dataInicio, dataFim, ct)
                        : await _repository.ListarTodosAsync(ct);

                return entidades.Select(entity =>
                {
                    var fabricante = entity.NomeDoFabricante ?? string.Empty;
                    var fabricanteLower = fabricante.ToLowerInvariant();

                    var invoice = string.IsNullOrWhiteSpace(entity.NumeroDaNFSe)
                                  && fabricanteLower.Contains("red hot")
                        ? (entity.NumeroDocumento ?? string.Empty)
                        : (entity.NumeroDaNFSe ?? string.Empty);

                    var dataEmissao = entity.DataEmissao == default
                        ? DateTime.MinValue
                        : entity.DataEmissao;

                    var valorInvoice = (entity.Resumos?.Sum(r => r?.ValorPagamentoTotal ?? 0m)) ?? 0m;

                    return new MonitoramentoSppBimerInvoceDto
                    {
                        Id = entity.CdEmpresa,
                        NumeroAlterData = entity.NumeroDocumento ?? string.Empty,
                        PedidoSpp = invoice,
                        Fabricante = fabricante,
                        FabricanteId = entity.FabricanteId?.ToString(),
                        Estoque = entity.Estoque ?? string.Empty,
                        DataEmissao = dataEmissao,
                        ValorInvoice = valorInvoice,
                        StatusIntegracao = entity.StatusIntegracaoAlterData ?? "N",
                        ObservacaoErro = entity.StatusIntegracaoAlterDataObs ?? string.Empty
                    };
                });
            }
            catch (Exception ex)
            {
                var details = $"status={status ?? "<null>"}; " +
                              $"dataInicio={(dataInicio?.ToString("yyyy-MM-dd") ?? "<null>")}; " +
                              $"dataFim={(dataFim?.ToString("yyyy-MM-dd") ?? "<null>")}";
                throw new SppBimerInvoceServiceException(
                    $"{nameof(MonitorSppBimerAppService)}.{nameof(ObterMonitoramentosAsync)}",
                    details,
                    ex);
            }
        }

        public async Task<string> ReprocessarAsync(ReprocessarBimerRequestDto request, CancellationToken ct)
        {
            try
            {
                return await _repository.ReprocessarAsync(request, ct);
            }
            catch (Exception ex)
            {
                throw new SppBimerInvoceServiceException(
                    $"{nameof(MonitorSppBimerAppService)}.{nameof(ReprocessarAsync)}",
                    details: null,
                    inner: ex);
            }
        }
    }
}