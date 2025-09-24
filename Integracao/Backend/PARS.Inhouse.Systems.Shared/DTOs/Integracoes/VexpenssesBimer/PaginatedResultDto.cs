/// <summary>
/// Representa um resultado paginado genérico contendo uma lista de itens e a contagem total.
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados na página.</typeparam>
public class PaginatedResultDto<T>
{
    /// <summary>
    /// Lista de itens da página atual.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Quantidade total de registros disponíveis (sem paginação).
    /// </summary>
    public int TotalCount { get; set; }
}
