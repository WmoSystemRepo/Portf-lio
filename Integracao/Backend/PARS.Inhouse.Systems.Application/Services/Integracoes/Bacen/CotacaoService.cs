using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen;
using PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.Bacen;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.Bacen;
using System.Text.Json;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.Bacen
{
    public class CotacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly CotacaoDolarRepository _cotacaoDolarRepository;

        public CotacaoService(HttpClient httpClient, CotacaoDolarRepository cotacaoDolarRepository)
        {
            _httpClient = httpClient;
            _cotacaoDolarRepository = cotacaoDolarRepository;
        }

        public async Task<List<CotacaoMoedaDto>> ObterCotacoesPorPeriodoAsync(string codigoMoeda, DateTime dataInicial, DateTime dataFinal, CancellationToken cancellationToken)
        {
            try
            {
                var cotacoesAcumuladas = new List<CotacaoMoedaDto>();

                var dataAtual = dataInicial.Date;
                var dataFim = dataFinal.Date;

                while (dataAtual <= dataFim)
                {
                    string dataFormatada = dataAtual.ToString("MM-dd-yyyy");
                    string url = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/" +
                                 $"CotacaoMoedaPeriodoFechamento(codigoMoeda=@codigoMoeda," +
                                 $"dataInicialCotacao=@dataInicialCotacao,dataFinalCotacao=@dataFinalCotacao)?" +
                                 $"@codigoMoeda='{codigoMoeda}'&@dataInicialCotacao='{dataFormatada}'" +
                                 $"&@dataFinalCotacao='{dataFormatada}'" +
                                 $"&$format=json&$select=cotacaoCompra,cotacaoVenda,dataHoraCotacao,tipoBoletim";

                    cancellationToken.ThrowIfCancellationRequested();
                    var response = await _httpClient.GetAsync(url, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        dataAtual = dataAtual.AddDays(1);
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync(cancellationToken);
                    var result = JsonSerializer.Deserialize<CotacaoMoedaResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result?.value != null && result.value.Any())
                    {
                        cotacoesAcumuladas.AddRange(result.value);

                        await _cotacaoDolarRepository.SalvarCotacaoMoeda(result.value, json, codigoMoeda, cancellationToken);
                    }

                    dataAtual = dataAtual.AddDays(1);
                }

                return cotacoesAcumuladas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CotacaoListagemDto> ListarCotacoes(CancellationToken cancellationToken)
        {
            try
            {
                var ultimasCotacoes = await _cotacaoDolarRepository.ListarUltimasCotacoesAsync(2, cancellationToken);

                if (ultimasCotacoes == null || ultimasCotacoes.Count < 2)
                {
                    return null;
                }

                var cotacaoAtual = ultimasCotacoes[0];
                var cotacaoAnterior = ultimasCotacoes[1];

                cancellationToken.ThrowIfCancellationRequested();
                decimal variacao = cotacaoAtual.CotacaoVenda - cotacaoAnterior.CotacaoVenda;
                string direcao = variacao > 0 ? "Subiu" : variacao < 0 ? "Desceu" : "Estável";

                var dto = new CotacaoListagemDto
                {
                    CotacaoAtual = new CotacaoComVariacaoDto
                    {
                        CotacaoCompra = cotacaoAtual.CotacaoCompra,
                        CotacaoVenda = cotacaoAtual.CotacaoVenda,
                        DataHoraCotacao = cotacaoAtual.DataHoraCotacao,
                        TipoBoletim = cotacaoAtual.TipoBoletim,
                        Direcao = direcao,
                        Margem = Math.Abs(variacao)
                    },

                    Historico = await _cotacaoDolarRepository.ListarUltimaCotacaoPorDiaAsync(cancellationToken)
                };

                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

public class CotacaoListagemDto
{
    public CotacaoComVariacaoDto CotacaoAtual { get; set; }
    public List<IntegracaoBacenCotacaoMoeda> Historico { get; set; }
}

public class CotacaoComVariacaoDto
{
    public decimal CotacaoCompra { get; set; }
    public decimal CotacaoVenda { get; set; }
    public DateTime DataHoraCotacao { get; set; }
    public string TipoBoletim { get; set; }
    public string Direcao { get; set; } // "Subiu", "Desceu", "Estável"
    public decimal Margem { get; set; } // Diferença
}

public class CotacaoMoedaResponse
{
    public List<CotacaoMoedaDto> value { get; set; }
}
