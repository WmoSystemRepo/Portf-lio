/// <summary>
/// DTO que representa os dados básicos de uma despesa/título financeiro a ser processado.
/// </summary>
public class DespesaTituloDto
{
    /// <summary>
    /// Valor monetário total da despesa.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Identificador do tipo de despesa (ex: viagem, refeição, hospedagem).
    /// </summary>
    public string? ExpenseTypeId { get; set; }

    /// <summary>
    /// Identificador da empresa responsável pelo pagamento.
    /// </summary>
    public string? PayingCompanyId { get; set; }

    /// <summary>
    /// Identificador do método de pagamento utilizado (ex: cartão, transferência).
    /// </summary>
    public string? PaymentMethodId { get; set; }

    /// <summary>
    /// Observações adicionais sobre a despesa.
    /// </summary>
    public string? Observation { get; set; }

    /// <summary>
    /// Título ou descrição breve da despesa.
    /// </summary>
    public string? Title { get; set; }
}
