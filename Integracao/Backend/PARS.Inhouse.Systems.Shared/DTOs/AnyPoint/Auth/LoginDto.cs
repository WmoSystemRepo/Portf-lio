using System.ComponentModel.DataAnnotations;

namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Auth
{
    /// <summary>
    /// DTO utilizado para autenticação de usuários no sistema via AnyPoint.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// E-mail do usuário para login. Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário para autenticação. Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; } = string.Empty;
    }
}
