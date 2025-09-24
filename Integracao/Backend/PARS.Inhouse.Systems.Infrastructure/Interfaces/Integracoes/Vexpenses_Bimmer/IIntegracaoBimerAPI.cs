namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IIntegracaoBimerAPI
    {
        Task<string> CriarTituloAPagar(string bimerRequest, string uri, string token, CancellationToken cancellationToken = default);
        Task<string> AuthenticateAsync(FormUrlEncodedContent content, string uri, CancellationToken cancellationToken = default);
        Task<string> ReauthenticateAsync(FormUrlEncodedContent content, string uri, CancellationToken cancellationToken = default);
        Task<string> ObterBimerIdentificadorPessoaPorCPF(string uri, string token, CancellationToken cancellationToken = default);
    }
}
