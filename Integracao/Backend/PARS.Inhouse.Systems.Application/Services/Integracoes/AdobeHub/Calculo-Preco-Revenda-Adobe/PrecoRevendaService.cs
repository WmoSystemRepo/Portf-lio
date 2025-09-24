using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using System.Globalization;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class PrecoRevendaService : IPrecoRevendaService
    {
        public PrecoRevendaService() { }

        public Task<decimal> CalcularPrecoRevendaAsync(decimal fob, string nivel, IndicePrecoRevendaDto idx, CancellationToken cancellationToken = default)
        {
            if (fob <= 0)
                throw new ArgumentException("FOB deve ser maior que zero.", nameof(fob));

            // ✅ Impostos conforme legado: PIS + COFINS + ISS
            decimal impostos = idx.PIS + idx.COFINS + idx.ISS;

            // ✅ Margem por nível com regra compatível ao legado ("Level 1 " com espaço)
            decimal margem = EhLevel1_LegacyCompatible(nivel) ? idx.ProdNivel1 : idx.OutrosProd;

            // Fórmula do legado
            decimal precoComCusto = fob + (fob * idx.CustoOperacional / 100m);
            decimal precoComMargem = DividirPeloFator(precoComCusto, margem);
            decimal precoFinal = DividirPeloFator(precoComMargem, impostos);

            // ✅ Arredondamento final igual ao legado (2 casas, AwayFromZero)
            decimal resultado = decimal.Round(precoFinal, 2, MidpointRounding.AwayFromZero);
            return Task.FromResult(resultado);
        }

        public async Task AplicarPrecoRevendaAsync(int fabricanteId, string segmento, IList<Dictionary<string, object?>> linhas, IndicePrecoRevendaDto idx, CancellationToken cancellationToken = default)
        {
            if (linhas == null || linhas.Count == 0) return;

            const string nomeColunaFob = "Partner Price";
            const string nomeColunaLevel = "Level Detail";

            foreach (var linha in linhas)
            {
                var chaveFob = linha.Keys.FirstOrDefault(k => Normalizar(k).Contains(Normalizar(nomeColunaFob)));
                var chaveLevel = linha.Keys.FirstOrDefault(k => Normalizar(k).Contains(Normalizar(nomeColunaLevel)));

                if (string.IsNullOrWhiteSpace(chaveFob))
                {
                    linha["PrecoRevenda"] = "";
                    linha["FOB"] = "";
                    continue;
                }

                if (linha.TryGetValue(chaveFob, out var fobObj) &&
                    decimal.TryParse(fobObj?.ToString()?.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out var fob) &&
                    fob > 0)
                {
                    string nivel = linha.TryGetValue(chaveLevel ?? "", out var levelObj) ? levelObj?.ToString() ?? "" : "";
                    var preco = await CalcularPrecoRevendaAsync(fob, nivel, idx, cancellationToken);

                    linha["PrecoRevenda"] = preco.ToString("F2", CultureInfo.InvariantCulture);
                    linha["FOB"] = fob.ToString("F2", CultureInfo.InvariantCulture);
                }
                else
                {
                    linha["PrecoRevenda"] = "";
                    linha["FOB"] = "";
                }
            }
        }

        private static decimal DividirPeloFator(decimal baseValor, decimal percentual)
        {
            var fator = 1m - (percentual / 100m);
            if (fator <= 0m)
                throw new InvalidOperationException($"Percentual inválido: {percentual}. O fator (1 - p/100) deve ser > 0.");
            return baseValor / fator;
        }

        private static bool EhLevel1_LegacyCompatible(string? nivel)
        {
            if (string.IsNullOrWhiteSpace(nivel)) return false;
            var s = nivel.TrimStart();
            return s.StartsWith("Level 1 ", StringComparison.InvariantCultureIgnoreCase);
        }

        private static string Normalizar(string s)
        {
            return s?.Trim().ToLowerInvariant().Replace(" ", "").Replace("-", "").Replace("_", "") ?? "";
        }
    }
}
