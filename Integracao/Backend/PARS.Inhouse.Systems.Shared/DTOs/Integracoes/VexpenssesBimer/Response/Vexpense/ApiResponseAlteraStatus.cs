using PARS.Inhouse.Systems.Shared.Enums.Integracoes.VexpessesBimer.Vexpenses;

/// <summary>
/// Representa a resposta da API ao tentar alterar o status de um relatório ou título no VExpenses.
/// </summary>
public class ApiResponseAlteraStatus
{
    /// <summary>
    /// Dados detalhados da resposta da API.
    /// </summary>
    public ResponseData? data { get; set; }
}

/// <summary>
/// Estrutura detalhada com os dados retornados pela operação de alteração de status.
/// </summary>
public class ResponseData
{
    /// <summary>
    /// Lista de erros retornados, se houver, agrupados por campo.
    /// </summary>
    public Dictionary<string, List<string>>? errors { get; set; }

    /// <summary>
    /// Identificador principal da resposta (ID interno do título).
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// ID externo do relatório, se houver.
    /// </summary>
    public int? external_id { get; set; }

    /// <summary>
    /// ID do usuário associado à ação.
    /// </summary>
    public int? user_id { get; set; }

    /// <summary>
    /// ID do dispositivo utilizado (se aplicável).
    /// </summary>
    public int? device_id { get; set; }

    /// <summary>
    /// Descrição do título ou relatório.
    /// </summary>
    public string? description { get; set; }

    /// <summary>
    /// Status atual do relatório/título.
    /// </summary>
    public StatusRelatorioVexpensses status { get; set; }

    /// <summary>
    /// ID do estágio de aprovação (workflow).
    /// </summary>
    public int? approval_stage_id { get; set; }

    /// <summary>
    /// ID do usuário que aprovou, se já aprovado.
    /// </summary>
    public int? approval_user_id { get; set; }

    /// <summary>
    /// Data de aprovação em formato string (ex: yyyy-MM-dd).
    /// </summary>
    public string? approval_date { get; set; }

    /// <summary>
    /// ID da empresa responsável pelo pagamento.
    /// </summary>
    public int? payment_company_id { get; set; }

    /// <summary>
    /// Data em que o pagamento foi registrado (se aplicável).
    /// </summary>
    public string? payment_date { get; set; }

    /// <summary>
    /// ID da forma de pagamento selecionada.
    /// </summary>
    public int? payment_method_id { get; set; }

    /// <summary>
    /// Observações adicionais sobre o processo.
    /// </summary>
    public string? observation { get; set; }

    /// <summary>
    /// Indica se o título está ativo ou visível.
    /// </summary>
    public bool? on { get; set; }

    /// <summary>
    /// Justificativa inserida durante a aprovação ou alteração de status.
    /// </summary>
    public string? justification { get; set; }

    /// <summary>
    /// Link direto para o PDF do título/relatório.
    /// </summary>
    public Uri? pdf_link { get; set; }

    /// <summary>
    /// Link direto para exportação em Excel.
    /// </summary>
    public Uri? excel_link { get; set; }

    /// <summary>
    /// Data de criação do registro no sistema remoto.
    /// </summary>
    public string? create_at { get; set; }

    /// <summary>
    /// Data da última atualização do registro.
    /// </summary>
    public string? updated_at { get; set; }
}

/// <summary>
/// Reservado para futuras definições de estrutura de erro (não utilizado atualmente).
/// </summary>
public class ErrorData
{
}
