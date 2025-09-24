namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu
{
    /// <summary>
    /// DTO que representa a estrutura de um item de menu na aplicação.
    /// Pode ser um menu principal ou um submenu, com suporte a ordenação e hierarquia.
    /// </summary>
    public class MenuDto
    {
        /// <summary>
        /// Identificador único do menu.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do item de menu a ser exibido na interface.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Rota associada ao menu, utilizada para navegação.
        /// </summary>
        public string? Rota { get; set; }

        /// <summary>
        /// Ícone que representa o menu visualmente (ex: nome de ícone do Material Icons ou outro set).
        /// </summary>
        public string? Icone { get; set; }

        /// <summary>
        /// Número de ordenação do menu na exibição.
        /// Quanto menor o valor, mais acima o item será posicionado.
        /// </summary>
        public int? OrdenacaoMenu { get; set; }

        /// <summary>
        /// Indica se o item é um menu principal (true) ou um submenu (false).
        /// </summary>
        public bool? EhMenuPrincipal { get; set; }

        /// <summary>
        /// Referência (ID) ao menu principal pai, caso este item seja um submenu.
        /// </summary>
        public int? SubMenuReferenciaPrincipal { get; set; }

        /// <summary>
        /// Data de criação do item de menu (como string ou formato ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição do menu (como string ou formato ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }
    }
}
