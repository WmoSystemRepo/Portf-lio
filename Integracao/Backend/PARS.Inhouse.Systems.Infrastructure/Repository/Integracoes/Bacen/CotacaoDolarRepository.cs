using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.Bacen;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen
{
    public class CotacaoDolarRepository
    {
        private readonly Context _context;

        public CotacaoDolarRepository(Context context)
        {
            _context = context;
        }

        public async Task SalvarCotacaoMoeda(List<CotacaoMoedaDto> cotacoesMoedasDto, string response, string codigoMoeda, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var cotacaoMoedaDto in cotacoesMoedasDto)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var dataCotacao = DateTime.Parse(cotacaoMoedaDto.dataHoraCotacao);

                    bool cotacaoExiste = _context.IntegracaoBacenCotacaoMoeda.Where(x => x.DataHoraCotacao == dataCotacao).Any();

                    if (cotacaoExiste) continue;

                    IntegracaoBacenCotacaoMoeda cotacaoMoeda = new IntegracaoBacenCotacaoMoeda
                    {
                        CotacaoCompra = cotacaoMoedaDto.cotacaoCompra,
                        CotacaoVenda = cotacaoMoedaDto.cotacaoVenda,
                        DataHoraCotacao = DateTime.Parse(cotacaoMoedaDto.dataHoraCotacao),
                        TipoBoletim = cotacaoMoedaDto.tipoBoletim,
                        Response = response,
                        DataHoraIntegracao = ObterDataHoraBrasilia(),
                        CodigoMoeda = codigoMoeda
                    };
                    _context.IntegracaoBacenCotacaoMoeda.Add(cotacaoMoeda);
                    await _context.SaveChangesAsync();
                }

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<IntegracaoBacenCotacaoMoeda>> ListarUltimasCotacoesAsync(int take, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var cotacoes = await _context.IntegracaoBacenCotacaoMoeda.OrderByDescending(x => x.DataHoraCotacao).Take(take).ToListAsync();

            return cotacoes;
        }

        public async Task<List<IntegracaoBacenCotacaoMoeda>> ListarUltimaCotacaoPorDiaAsync(CancellationToken cancellationToken)
        {
            // Executa no banco: traz só os últimos registros por dia
            var cotacoesPorDia = await _context.IntegracaoBacenCotacaoMoeda
                .AsNoTracking()
                .GroupBy(c => c.DataHoraCotacao.Date)
                .Select(g => g
                    .OrderByDescending(c => c.DataHoraCotacao)
                    .FirstOrDefault()
                )
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();
            // Executa em memória: realiza a projeção com os campos desejados
            return cotacoesPorDia
                .Where(c => c != null)
                .OrderBy(c => c.DataHoraCotacao)
                .Select(c => new IntegracaoBacenCotacaoMoeda
                {
                    CodigoMoeda = c.CodigoMoeda,
                    DataHoraIntegracao = c.DataHoraIntegracao,
                    Id = c.Id,
                    Response = c.Response,
                    TipoBoletim = c.TipoBoletim,
                    CotacaoCompra = c.CotacaoCompra,
                    CotacaoVenda = c.CotacaoVenda,
                    DataHoraCotacao = c.DataHoraCotacao
                })
                .ToList();
        }



        public static DateTime ObterDataHoraBrasilia()
        {
            try
            {
                // Para sistemas Unix/Linux (e Windows com suporte)
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Para Windows
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            catch (Exception)
            {
                // Se não conseguir identificar o fuso, retorna UTC como fallback (evite se possível)
                return DateTime.UtcNow;
            }
        }

    }
}
