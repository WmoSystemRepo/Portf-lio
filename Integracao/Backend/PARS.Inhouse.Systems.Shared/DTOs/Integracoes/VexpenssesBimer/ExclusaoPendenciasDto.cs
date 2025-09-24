/// <summary>
/// DTO utilizado para representar a solicitação de exclusão de pendências.
/// </summary>
public class ExclusaoPendenciasDto
{
    /// <summary>
    /// Justificativa da exclusão fornecida pelo usuário.
    /// </summary>
    public string Justificativa { get; set; } = string.Empty;

    /// <summary>
    /// Lista de pendências que foram excluídas.
    /// </summary>
    public List<PendenciaDto>? RegistrosExcluidos { get; set; } = new();

    /// <summary>
    /// Nome ou identificador do usuário que executou a exclusão.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora em que a exclusão foi realizada.
    /// </summary>
    public DateTime DataHora { get; set; }
}

/// <summary>
/// DTO que representa uma pendência individual.
/// </summary>
public class PendenciaDto
{
    /// <summary>
    /// Identificador da resposta relacionada à pendência.
    /// </summary>
    public int IdResponse { get; set; }

    /// <summary>
    /// Identificador do usuário responsável pela pendência.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Descrição ou título da pendência.
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Valor monetário associado à pendência.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Data de cadastro da pendência.
    /// </summary>
    public DateTime DataCadastro { get; set; }

    /// <summary>
    /// Observações adicionais da pendência.
    /// </summary>
    public string? Observacao { get; set; }

    /// <summary>
    /// Texto ou payload de resposta relacionado (ex: JSON de erro).
    /// </summary>
    public string? Response { get; set; }
}
