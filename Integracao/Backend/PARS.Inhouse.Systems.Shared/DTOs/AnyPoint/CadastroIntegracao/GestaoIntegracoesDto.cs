namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.CadastroIntegracao
{
    /// <summary>
    /// DTO utilizado para representar informações de gestão de integrações entre projetos no AnyPoint.
    /// </summary>
    public class GestaoIntegracoesDto
    {
        /// <summary>
        /// Identificador único da integração.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da integração configurada.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Nome do projeto de origem da integração.
        /// </summary>
        public string ProjetoOrigem { get; set; } = string.Empty;

        /// <summary>
        /// Nome do projeto de destino da integração.
        /// </summary>
        public string ProjetoDestino { get; set; } = string.Empty;

        /// <summary>
        /// Data de criação do registro de integração (formato: string).
        /// </summary>
        public string DataCriacao { get; set; } = string.Empty;

        /// <summary>
        /// Data da última edição do registro de integração (formato: string).
        /// </summary>
        public string DataEdicao { get; set; } = string.Empty;
    }
}
