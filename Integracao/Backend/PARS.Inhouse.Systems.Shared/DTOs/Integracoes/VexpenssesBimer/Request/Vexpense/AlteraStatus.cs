/// <summary>
/// DTO utilizado para alteração de status de um registro financeiro,
/// permitindo informar a data de pagamento e um comentário adicional.
/// </summary>
public class AlteraStatus
{
    /// <summary>
    /// Data em que o pagamento foi realizado.
    /// Formato sugerido: "yyyy-MM-dd" ou "yyyy-MM-ddTHH:mm:ss".
    /// </summary>
    public string? payment_date { get; set; }

    /// <summary>
    /// Comentário ou observação sobre a alteração de status.
    /// Pode ser utilizado para registrar o motivo ou identificação manual.
    /// </summary>
    public string? comment { get; set; }
}
