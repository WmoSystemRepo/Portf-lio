namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento
{
    /// <summary>
    /// DTO utilizado para representar o vínculo entre um mapeamento e uma integração no contexto do AnyPoint.
    /// </summary>
    public class MapeamentoIntegracaoDto
    {
        /// <summary>
        /// Identificador único do registro de mapeamento de integração.
        /// Pode ser nulo em operações de criação.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identificador do mapeamento associado.
        /// </summary>
        public int MapeamentoId { get; set; }

        /// <summary>
        /// Identificador da integração associada.
        /// </summary>
        public int IntegracaoId { get; set; }

        /// <summary>
        /// Data de criação do vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição do vínculo (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Indica se o vínculo está ativo.
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
