namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO que representa informações básicas de um usuário no sistema.
    /// Pode ser utilizado para exibição em listagens, seleções ou componentes de identificação de usuário.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Nome do usuário, usado para exibição.
        /// </summary>
        public string? NomeUsuario { get; set; }

        /// <summary>
        /// Identificador único do usuário (ex: ID do Identity).
        /// </summary>
        public string? IdUsuario { get; set; }
    }
}
