using System.Text.Json.Serialization;

/// <summary>
/// DTO utilizado para o registro de um novo usuário no sistema.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Endereço de e-mail do novo usuário.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>
    /// Nome de usuário que será utilizado no login.
    /// </summary>
    [JsonPropertyName("userName")]
    public required string UserName { get; set; }

    /// <summary>
    /// Senha para acesso ao sistema.
    /// </summary>
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    /// <summary>
    /// Número de telefone para contato ou autenticação.
    /// </summary>
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Define se o usuário pode visualizar dados.
    /// </summary>
    public bool PodeLer { get; set; }

    /// <summary>
    /// Define se o usuário pode adicionar ou editar informações.
    /// </summary>
    public bool PodeEscrever { get; set; }

    /// <summary>
    /// Define se o usuário pode excluir registros.
    /// </summary>
    public bool PodeRemover { get; set; }

    /// <summary>
    /// Define se o usuário tem acesso às configurações do sistema.
    /// </summary>
    public bool PodeVerConfiguracoes { get; set; }
}
