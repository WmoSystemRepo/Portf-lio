namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Response
{
    /// <summary>
    /// Representa uma pendência encontrada durante a validação de importação de planilha.
    /// </summary>
    public class PendenciaImportacaoDto
    {
        /// <summary>
        /// Número da linha na planilha (começa em 1).
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Nome da coluna que gerou o erro.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// Mensagem descritiva do problema encontrado.
        /// </summary>
        public string Message { get; set; }
    }
}
