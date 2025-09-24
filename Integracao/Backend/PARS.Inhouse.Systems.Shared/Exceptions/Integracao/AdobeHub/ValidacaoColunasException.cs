namespace PARS.Inhouse.Systems.Shared.Exceptions.Integracao.AdobeHub
{
    /// <summary>
    /// Exceção lançada quando as colunas da planilha importada não correspondem ao template esperado.
    /// </summary>
    public class ValidacaoColunasException : Exception
    {
        /// <summary>
        /// Detalhes do erro de validação, contendo colunas faltantes, colunas extras e mensagem descritiva.
        /// </summary>
        public ColunaValidacaoErroDto Detalhes { get; }

        /// <summary>
        /// Construtor da exceção que aceita os detalhes do erro de validação.
        /// </summary>
        /// <param name="detalhes">Objeto com as colunas faltantes, extras e mensagem personalizada.</param>
        public ValidacaoColunasException(ColunaValidacaoErroDto detalhes)
            : base(detalhes?.Mensagem ?? "Erro de validação de colunas.")
        {
            Detalhes = detalhes;
        }
    }
}
