using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class CalculoPrecoService
    {
        private readonly IPrecoRevendaService _precoRevendaService;
        private readonly ConfiguracoesService _configuracoesService;
        private readonly IRegrasExclusaoService _regrasService;

        public CalculoPrecoService(
            IPrecoRevendaService precoRevendaService,
            ConfiguracoesService configuracoesService,
            IRegrasExclusaoService regrasService)
        {
            _precoRevendaService = precoRevendaService;
            _configuracoesService = configuracoesService;
            _regrasService = regrasService;
        }

        public async Task<CalcularPrecoResponseDto> Handle(CalcularPrecoRequestDto request, CancellationToken ct)
        {
            // Mapear linhas da requisição
            var linhas = request.Linhas
                .Select(x => new Dictionary<string, object?>
                {
                    ["Part Number"] = x.PartNumber,
                    ["Level Detail"] = x.LevelDetail ?? "",
                    ["Partner Price"] = x.FOB
                })
                .ToList();

            // Obter regras de exclusão do Viewer
            var regras = (await _regrasService.ObterRegrasAsync(request.FabricanteId, request.Segmento, ct)).ToList();

            // Aplicar filtro das regras do Viewer
            linhas = FiltrarPorRegrasViewer(linhas, regras);

            // Obter configurações de cálculo
            var configuracoes = await _configuracoesService.Handle(request.FabricanteId, request.Segmento, ct);
            var usarMargemFixa = configuracoes.MetodoMargemAdobe == 'F';
            var margem = configuracoes.MargemFixa ?? 0;

            var indices = new IndicePrecoRevendaDto
            {
                PIS = configuracoes.PIS ?? 0,
                COFINS = configuracoes.COFINS ?? 0,
                ISS = configuracoes.ISS ?? 0,
                CustoOperacional = configuracoes.CustoOperacional ?? 0,
                ProdNivel1 = usarMargemFixa ? margem : configuracoes.ProdNivel1 ?? 0,
                OutrosProd = usarMargemFixa ? margem : configuracoes.OutrosProd ?? 0,
                ICMS = 0,
                Marketing = 0,
                Outros = 0
            };

            // Aplicar cálculo de preço
            await _precoRevendaService.AplicarPrecoRevendaAsync(request.FabricanteId, request.Segmento, linhas, indices, ct);

            // Montar resposta final
            var respLinhas = new List<CalcularPrecoResponseDto.LinhaCalculada>(linhas.Count);
            foreach (var l in linhas)
            {
                var part = l.TryGetValue("Part Number", out var p) ? p?.ToString() ?? "" : "";
                var fob = l.TryGetValue("FOB", out var f) && decimal.TryParse(f?.ToString(), out var fobDec) ? fobDec : 0m;
                var preco = l.TryGetValue("PrecoRevenda", out var pr) && decimal.TryParse(pr?.ToString(), out var precoDec) ? precoDec : 0m;

                var observacao = new List<string>();

                if (fob == 0)
                    observacao.Add("[FOB zero]");

                if (preco == 0)
                    observacao.Add("[Preço revenda zero]");

                observacao.Add(usarMargemFixa ? "[Margem Fixa aplicada]" : "[Margem Variável aplicada]");

                respLinhas.Add(new CalcularPrecoResponseDto.LinhaCalculada
                {
                    PartNumber = part,
                    FOB = fob,
                    PrecoRevendaUS = preco,
                    Observacao = string.Join(" ", observacao)
                });
            }

            return new CalcularPrecoResponseDto
            {
                Total = respLinhas.Count,
                Linhas = respLinhas
            };
        }

        private List<Dictionary<string, object?>> FiltrarPorRegrasViewer(
            List<Dictionary<string, object?>> linhas,
            List<RegraViewerDto> regras)
        {
            var excluir = regras
                .Where(r => r.Tipo == "ExcluirProduto" && r.ColunaTabela == "Part Number")
                .Select(r => r.Item?.Trim().ToLowerInvariant())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToHashSet();

            var incluirSomente = regras
                .Where(r => r.Tipo == "IncluirSomente" && r.ColunaTabela == "Part Number")
                .Select(r => r.Item?.Trim().ToLowerInvariant())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToHashSet();

            return linhas
                .Where(l =>
                {
                    var partNumber = l.TryGetValue("Part Number", out var val)
                        ? val?.ToString()?.Trim().ToLowerInvariant()
                        : null;

                    if (string.IsNullOrWhiteSpace(partNumber))
                        return false;

                    if (incluirSomente.Count > 0 && !incluirSomente.Contains(partNumber))
                        return false;

                    if (excluir.Contains(partNumber))
                        return false;

                    return true;
                })
                .ToList();
        }
    }
}
