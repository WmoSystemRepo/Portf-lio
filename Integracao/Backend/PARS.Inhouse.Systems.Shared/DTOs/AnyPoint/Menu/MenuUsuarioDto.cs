namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Menu
{
    /// <summary>
    /// DTO que representa a associação entre um item de menu e um usuário específico.
    /// Utilizado para definir menus personalizados por usuário no sistema AnyPoint.
    /// </summary>
    public class MenuUsuarioDto
    {
        /// <summary>
        /// Identificador único da associação entre o menu e o usuário.
        /// Pode ser nulo em operações de criação.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identificador do menu associado ao usuário.
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Identificador do usuário (normalmente o ID do Identity).
        /// </summary>
        public string UsuarioId { get; set; } = string.Empty;

        /// <summary>
        /// Data de criação da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição da associação (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Indica se a associação entre o menu e o usuário está ativa.
        /// </summary>
        public bool Ativo { get; set; } = true;
    }
}
