namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public interface IIndicesAdobeRepository
    {
        Task<(decimal PIS, decimal COFINS, decimal ISS, decimal CustoOperacional, decimal ProdNivel1, decimal OutrosProd, decimal? MargemMinima)>
            ObterAsync(int fabricanteId, string segmento, CancellationToken ct);
    }
}
