/// <summary>
/// DTO que representa a resposta de autenticação de um usuário ou cliente,
/// contendo os tokens de acesso e renovação, além de informações auxiliares.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Token de acesso (JWT) que deve ser enviado nas requisições autenticadas.
    /// </summary>
    public required string access_token { get; set; }

    /// <summary>
    /// Tipo do token de autenticação.
    /// Geralmente é "Bearer".
    /// </summary>
    public required string token_type { get; set; }

    /// <summary>
    /// Tempo de expiração do token em segundos.
    /// </summary>
    public required int expires_in { get; set; }

    /// <summary>
    /// Token de renovação (refresh token) para obter novos access tokens sem reautenticar.
    /// </summary>
    public required string refresh_token { get; set; }

    /// <summary>
    /// Nome de usuário autenticado (pode ser o login ou identificador).
    /// </summary>
    public required string username { get; set; }

    /// <summary>
    /// Código de erro, se a autenticação falhar (opcional).
    /// </summary>
    public string? error { get; set; }

    /// <summary>
    /// Descrição detalhada do erro, útil para debugging ou logs.
    /// </summary>
    public string? error_description { get; set; }
}
