namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu
{
    /// <summary>
    /// DTO que representa a associação entre um item de menu e uma regra de acesso (permissão) no sistema AnyPoint.
    /// </summary>
    public class MenuRegraDto
    {
        /// <summary>
        /// Identificador único da associação entre o menu e a regra.
        /// Pode ser nulo em cenários de criação.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identificador do menu ao qual a regra está vinculada.
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Identificador da regra (role ou permissão) associada ao menu.
        /// Exemplo: "Admin", "PodeLerRelatorio", "Configuracoes".
        /// </summary>
        public string RegraId { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a associação está ativa.
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }
    }
}
