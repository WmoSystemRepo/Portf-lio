namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu
{
    /// <summary>
    /// DTO que representa o vínculo entre um item de menu e uma integração no sistema AnyPoint.
    /// Utilizado para configurar quais integrações estão associadas a menus específicos.
    /// </summary>
    public class MenuIntegracaoDto
    {
        /// <summary>
        /// Identificador único do vínculo menu-integracao.
        /// Pode ser nulo em operações de criação.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identificador do menu ao qual a integração está associada.
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Identificador da integração vinculada ao menu.
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
