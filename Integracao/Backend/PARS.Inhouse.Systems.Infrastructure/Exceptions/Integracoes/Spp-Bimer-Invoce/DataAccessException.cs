using System;

namespace PARS.Inhouse.Systems.Infrastructure.Exceptions.Integracoes.SppBimerInvoce
{
    /// <summary>
    /// Exceção padrão da camada de acesso a dados (Infra/Repository).
    /// Serve para padronizar a comunicação de erros e enriquecer o contexto
    /// (método e parâmetros) ao propagar para camadas superiores.
    /// </summary>
    public sealed class DataAccessException : Exception
    {
        /// <summary>
        /// Nome qualificado da operação que falhou (ex.: Repositorio.Metodo).
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// Texto com detalhes relevantes (ex.: valores de filtros/ids).
        /// </summary>
        public string? Details { get; }

        /// <param name="operation">Nome da operação/método onde o erro ocorreu.</param>
        /// <param name="details">Detalhes úteis (parâmetros, filtros, etc.).</param>
        /// <param name="inner">Exceção original capturada.</param>
        public DataAccessException(string operation, string? details, Exception inner)
            : base(BuildMessage(operation, details, inner), inner)
        {
            Operation = operation;
            Details = details;
        }

        private static string BuildMessage(string operation, string? details, Exception inner)
        {
            // Mensagem padronizada que sobe para a camada de serviço.
            // Mantém o InnerException com a stack original para diagnóstico.
            return details is null
                ? $"[Infra] DataAccessException em '{operation}'. Veja InnerException para detalhes."
                : $"[Infra] DataAccessException em '{operation}'. Detalhes: {details}. Veja InnerException para detalhes.";
        }
    }
}
