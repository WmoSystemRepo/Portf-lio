using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;
using System.Globalization;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer
{
    public class TituloRepository : IVexpenssesRepositorio
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public TituloRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<IntegracaoVexpenseTitulosRelatoriosStatus>> ObterTitulosAprovadosAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.IntegracaoVexpenseTitulosRelatoriosStatus
                                 .Where(x => x.Status == StatusRelatorioVexpensses.APROVADO.ToString())
                                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IntegracaoVexpenseTitulosRelatoriosStatus?> ObterTituloAprovadoPorIdAsync(int idResponse, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.IntegracaoVexpenseTitulosRelatoriosStatus
                                 .Where(x => x.IdResponse == idResponse && x.Status == StatusRelatorioVexpensses.APROVADO.ToString())
                                 .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IntegracaoBimmerInsercaoPendentes?> ObterPendentePorIdResponseAsync(int idResponse, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.IntegracaoBimmerInsercaoPendentes
                                 .Where(x => x.IdResponse == idResponse)
                                 .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IntegracaoBimmerInsercaoPendentes>> RecuperarTodosRegistrosPendentesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.IntegracaoBimmerInsercaoPendentes
                                                     .AsNoTracking()
                                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IntegracaoBimmerInsertOK> RecuperarTituloPagoPorIdAsync(int tituloId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.IntegracaoBimmerInsertOK
                             .Where(x => x.IdResponse == tituloId)
                             .FirstOrDefaultAsync();

                if (result == null)
                {
                    throw new KeyNotFoundException($"Nenhum registro encontrado para tituloId: {tituloId}");
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AnyPointDeparas?> RecuperarNaturezaLancamentoVexpenseAsync(string? paymentMethodId, CancellationToken cancellationToken)
        {
            var result = await _context.AnyPointDeparas
                .FirstOrDefaultAsync(x =>
                    x.Integracao == "VEXPENSE_BIMMER" &&
                    x.ValorOrigem == paymentMethodId &&
                    x.TipoExecucao == "API" &&
                    x.CampoOrigem == "EXPENSE_TYPE_ID");

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<AnyPointDeparas?> RecuperarDeParaEmpresaPagadoraAsync(string? payingCompanyId, CancellationToken cancellationToken)
        {
            var result = await _context.AnyPointDeparas
                .FirstOrDefaultAsync(x =>
                    x.Integracao == "VEXPENSE_BIMMER" &&
                    x.ValorOrigem == payingCompanyId &&
                    x.TipoExecucao == "API" &&
                    x.CampoOrigem == "PAYING_COMPANY_ID");

            if (result == null)
            {
                return null;
            }

            return result;
        }


        public async Task RegistrarTituloPagoAsync(IntegracaoBimmerInsertOK tituloPago, CancellationToken cancellationToken)
        {
            try
            {
                await _context.IntegracaoBimmerInsertOK.AddAsync(tituloPago);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RegistrarHistoricoInsercaoComErroAsync(IntegracaoBimmerHistoricoErrosInsercoes historico, CancellationToken cancellationToken)
        {
            try
            {
                await _context.IntegracaoBimmerHistoricoErrosInsercoes.AddAsync(historico);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SalvaOuAltera_IntegracaoVexpenseTitulosRelatoriosStatus_Async(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SalvarAlteracoesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<IntegracaoBimmerHistoricoErrosInsercoes>> RecuperarHistoricoErrosIntegracaoAsync(int pageNumber,
                                                                                                  int pageSize,
                                                                                                  string? search = null,
                                                                                                  CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var query = _context.IntegracaoBimmerHistoricoErrosInsercoes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var formats = new[] { "dd/MM/yyyy" };
                if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
                {
                    query = query.Where(r =>
                        r.DataTentativa.Value.Date == searchDate.Date);
                }
                else
                {
                    query = query.Where(r =>
                        r.IdResponse.ToString().Contains(search) ||
                        r.Tentativas.ToString().Contains(search) ||
                        r.MensagemErro.ToString().Contains(search));
                }
            }

            List<IntegracaoBimmerHistoricoErrosInsercoes> reports = await query
                    .OrderByDescending(r => r.DataTentativa)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            var dto = _mapper.Map<IReadOnlyList<IntegracaoBimmerHistoricoErrosInsercoes>>(reports);

            return dto;
        }

        public async Task<IReadOnlyList<IntegracaoBimmerInsertOK>> RecuperarTitulosEnviadosComSucessoAsync(int pageNumber,
                                                                                                  int pageSize,
                                                                                                  string? search = null,
                                                                                                  CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var query = _context.IntegracaoBimmerInsertOK.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var formats = new[] { "dd/MM/yyyy" };
                if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
                {
                    query = query.Where(r =>
                        r.DataCadastro.Value.Date == searchDate.Date);
                }
                else
                {
                    query = query.Where(r =>
                        r.IdResponse.ToString().Contains(search) ||
                        r.IdentificadorBimmer.ToString().Contains(search) ||
                        r.Valor.ToString().Contains(search) ||
                        r.Observacao.Contains(search));
                }
            }

            List<IntegracaoBimmerInsertOK> reports = await query
                    .OrderByDescending(r => r.DataCadastro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();
            var dto = _mapper.Map<IReadOnlyList<IntegracaoBimmerInsertOK>>(reports);

            return dto;
        }

        public async Task<AnyPointDeparas?> RecuperarMapeamentoCentroDeCustoAsync(string? payingCompanyId, CancellationToken cancellationToken)
        {
            var result = await _context.AnyPointDeparas
                .FirstOrDefaultAsync(x =>
                    x.Integracao == "VEXPENSE_BIMMER" &&
                    x.ValorOrigem == payingCompanyId &&
                    x.TipoExecucao == "API" &&
                    x.CampoOrigem == "PAYING_COMPANY_ID");

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<List<int>> ExcluirPendenciasPorIdsAsync(int[] ids, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (ids == null || ids.Length == 0)
                return new List<int>();

            var pendencias = await _context.IntegracaoBimmerInsercaoPendentes
                                           .Where(p => ids.Contains(p.IdResponse ?? -1))
                                           .ToListAsync(cancellationToken);

            if (!pendencias.Any())
                return new List<int>();

            var idsExcluidos = pendencias
                .Select(p => p.IdResponse ?? 0)
                .ToList();

            _context.IntegracaoBimmerInsercaoPendentes.RemoveRange(pendencias);
            await _context.SaveChangesAsync(cancellationToken);

            return idsExcluidos;
        }
    }
}