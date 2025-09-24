//// File: Shared/Utils/Integracao/AdobeHub/PrecoRevendaCalculator.cs
//using System;
//using System.Globalization;
//using System.Threading;
//using System.Threading.Tasks;
//using PARS.Inhouse.Systems.Core.Services.Interfaces; // IIndicePrecoRevendaAppService
//using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub; // IndicePrecoRevendaDto (ajuste se o nome diferir)

//namespace PARS.Inhouse.Systems.Shared.Utils.Integracao.AdobeHub
//{
//    /// <summary>
//    /// Calculadora de preço de revenda baseada em índices vindos do banco de dados.
//    /// </summary>
//    public interface IPrecoRevendaCalculator
//    {
//        /// <summary>
//        /// Calcula o preço de revenda usando índices dinâmicos do BD.
//        /// </summary>
//        Task<decimal> CalcularAsync(decimal fob, string nivel, CancellationToken cancellationToken = default);
//    }

//    /// <summary>
//    /// Implementação que busca os índices via <see cref="IIndicePrecoRevendaAppService"/>.
//    /// </summary>
//    public sealed class PrecoRevendaCalculator : IPrecoRevendaCalculator
//    {
//        private readonly IIndicePrecoRevendaAppService _indiceService;

//        public PrecoRevendaCalculator(IIndicePrecoRevendaAppService indiceService)
//        {
//            _indiceService = indiceService ?? throw new ArgumentNullException(nameof(indiceService));
//        }

//        /// <summary>
//        /// Calcula o preço de revenda baseado no valor FOB e nos percentuais de margem e impostos
//        /// obtidos do banco de dados.
//        /// </summary>
//        /// <param name="fob">Valor FOB</param>
//        /// <param name="nivel">Texto do campo 'Level Detail' (ex.: "Level 1 1 - 9")</param>
//        /// <param name="cancellationToken">Token de cancelamento</param>
//        /// <returns>Preço de revenda com até 8 casas decimais</returns>
//        public async Task<decimal> CalcularAsync(decimal fob, string nivel, CancellationToken cancellationToken = default)
//        {
//            if (fob <= 0)
//                throw new ArgumentException("FOB deve ser maior que zero.", nameof(fob));

//            // Busca índices atuais no BD (via serviço de aplicação)
//            var idx = await _indiceService.ObterIndiceAsync(cancellationToken);
//            if (idx == null)
//                throw new InvalidOperationException("Índices de preço de revenda não encontrados no banco de dados.");

//            // Espera-se que o DTO possua estes campos. Caso os nomes sejam diferentes,
//            // ajuste os acessos abaixo.
//            decimal custoOperacional = idx.CustoOperacional; // %
//            decimal pis = idx.Pis;                             // %
//            decimal cofins = idx.Cofins;                       // %
//            decimal iss = idx.Iss;                             // %

//            // Margem por nível (ex.: Level 1 usa uma margem específica)
//            decimal margem = EhLevel1(nivel) ? idx.MargemLevel1 : idx.MargemPadrao;

//            decimal impostos = pis + cofins + iss;            // % total

//            // Fórmula: aplica custo, depois margem, depois impostos (base líquido)
//            decimal precoComCusto = fob + (fob * custoOperacional / 100m);
//            decimal precoComMargem = DividirPeloFator(precoComCusto, margem);
//            decimal precoFinal = DividirPeloFator(precoComMargem, impostos);

//            return decimal.Round(precoFinal, 8);
//        }

//        // Evita repetição de (1 - x/100) e protege contra divisões inválidas
//        private static decimal DividirPeloFator(decimal baseValor, decimal percentual)
//        {
//            var fator = 1m - (percentual / 100m);
//            if (fator <= 0m)
//                throw new InvalidOperationException($"Percentual inválido: {percentual}. O fator (1 - p/100) deve ser > 0.");
//            return baseValor / fator;
//        }

//        private static bool EhLevel1(string nivel)
//        {
//            return !string.IsNullOrWhiteSpace(nivel)
//                   && nivel.Trim().StartsWith("Level 1", StringComparison.InvariantCultureIgnoreCase);
//        }
//    }
//}
