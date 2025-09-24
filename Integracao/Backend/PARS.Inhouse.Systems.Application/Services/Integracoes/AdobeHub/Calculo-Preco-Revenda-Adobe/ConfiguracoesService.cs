using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public sealed class ConfiguracoesService : IConfiguracoesService
    {
        private readonly ITabelaPrecosPublicacoesService _precosService;
        private readonly IRegrasExclusaoService _regrasService;

        public ConfiguracoesService(
            ITabelaPrecosPublicacoesService precosService,
            IRegrasExclusaoService regrasService)
        {
            _precosService = precosService;
            _regrasService = regrasService;
        }

        public async Task<ConfiguracoesResponseDto> Handle(
            int fabricanteId,
            string segmento,
            CancellationToken ct)
        {
            var regras = await _regrasService.ObterRegrasAsync(fabricanteId, segmento, ct);
            var config = await _precosService.ObterConfiguracaoValidaAsync(fabricanteId, ct);

            if (config == null)
                throw new InvalidOperationException("Configurações não encontradas para fabricante/segmento.");

            var metodo = regras.FirstOrDefault(r => r.Tipo == "MetodoMargem")?.Item;
            var margemFixa = regras.FirstOrDefault(r => r.Tipo == "MargemFixa")?.Item;

            return new ConfiguracoesResponseDto
            {
                MetodoMargemAdobe = !string.IsNullOrWhiteSpace(metodo) ? metodo[0] : 'N',
                MargemFixa = decimal.TryParse(margemFixa, out var mf) ? mf : null,
                PIS = config.PIS,
                COFINS = config.COFINS,
                ISS = config.ISS,
                CustoOperacional = config.CustoOperacional,
                ProdNivel1 = config.ProdNivel1,
                OutrosProd = config.OutrosProd,
                MargemMinima = config.MargemMinima
            };
        }
    }
}
