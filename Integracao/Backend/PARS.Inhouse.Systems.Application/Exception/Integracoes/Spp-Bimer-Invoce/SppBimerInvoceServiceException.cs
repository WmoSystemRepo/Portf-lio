using System;

namespace PARS.Inhouse.Systems.Application.Exceptions.Integracoes.SppBimerInvoce
{
    /// <summary>
    /// Exceção da camada de aplicação específica para o serviço
    /// de integração SPP ↔ Bimer (Invoices).
    /// 
    /// Padroniza a propagação de erros com contexto
    /// (operação e detalhes) até a camada de API.
    /// </summary>
    public sealed class SppBimerInvoceServiceException : Exception
    {
        /// <summary>Nome qualificado da operação (ex.: AppService.Metodo).</summary>
        public string Operation { get; }

        /// <summary>Detalhes úteis (parâmetros/cenário).</summary>
        public string? Details { get; }

        public SppBimerInvoceServiceException(string operation, string? details, Exception inner)
            : base(BuildMessage(operation, details), inner)
        {
            Operation = operation;
            Details = details;
        }

        private static string BuildMessage(string operation, string? details) =>
            details is null
                ? $"[App] SppBimerInvoceServiceException em '{operation}'. Veja InnerException."
                : $"[App] SppBimerInvoceServiceException em '{operation}'. Detalhes: {details}. Veja InnerException.";
    }
}
