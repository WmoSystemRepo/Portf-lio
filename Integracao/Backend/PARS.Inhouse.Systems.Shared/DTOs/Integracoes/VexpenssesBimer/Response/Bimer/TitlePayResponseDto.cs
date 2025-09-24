namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Bimer
{
    /// <summary>
    /// Representa a resposta do sistema após a tentativa de pagamento de títulos (invoices).
    /// Inclui informações sobre erros e os identificadores dos objetos processados.
    /// </summary>
    public class TitlePayResponseDto
    {
        /// <summary>
        /// Lista de erros detalhados que ocorreram durante o processamento.
        /// </summary>
        public required List<Error> Erros { get; set; }

        /// <summary>
        /// Lista de identificadores dos objetos que foram processados com sucesso.
        /// </summary>
        public required List<string> ListaObjetos { get; set; }
    }

    /// <summary>
    /// Representa os detalhes de um erro ocorrido durante o processo de integração.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Código do erro retornado pela API ou sistema externo.
        /// </summary>
        public required string ErrorCode { get; set; }

        /// <summary>
        /// Mensagem descritiva do erro.
        /// </summary>
        public required string ErrorMessage { get; set; }

        /// <summary>
        /// Causa provável do erro para facilitar o diagnóstico.
        /// </summary>
        public required string PossibleCause { get; set; }

        /// <summary>
        /// Stack trace (rastreamento de pilha) para análise técnica do erro.
        /// </summary>
        public required string StackTrace { get; set; }
    }
}
