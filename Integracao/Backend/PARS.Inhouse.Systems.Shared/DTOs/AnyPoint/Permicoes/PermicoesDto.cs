namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Permicoes
{
    /// <summary>
    /// DTO que representa uma permissão disponível no sistema.
    /// Utilizado para controle de acesso e vinculação a menus, usuários ou regras.
    /// </summary>
    public class PermicoesDto
    {
        /// <summary>
        /// Identificador único da permissão.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome descritivo da permissão.
        /// Exemplo: "PodeEditarUsuario", "VisualizarDashboard".
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Data de criação da permissão (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição da permissão (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }
    }
}
