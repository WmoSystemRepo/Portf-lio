/// <summary>
/// DTO utilizado para solicitar autenticação na API via credenciais.
/// Suporta fluxo baseado em clientId/clientSecret e senha de usuário.
/// </summary>
public class AuthRequestDto
{
    /// <summary>
    /// Identificador da aplicação cliente que está solicitando autenticação.
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// Chave secreta da aplicação cliente. Deve ser mantida confidencial.
    /// </summary>
    public required string ClientSecret { get; set; }

    /// <summary>
    /// Tipo do grant de autenticação.
    /// Valor padrão: "Senha" (para autenticação por usuário e senha).
    /// </summary>
    public string GrantType { get; set; } = "Senha";

    /// <summary>
    /// Nome de usuário (login) do usuário que deseja se autenticar.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Senha do usuário.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Valor nonce (valor único por requisição) para segurança contra replays ou rastreio.
    /// </summary>
    public required string Nonce { get; set; }
}
