using System.ComponentModel.DataAnnotations;

namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO utilizado para solicitar a troca de senha de um usuário no sistema.
    /// Inclui validações de formato, segurança e confirmação.
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Senha atual do usuário.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "Senha atual é obrigatória")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        /// <summary>
        /// Nova senha a ser definida.
        /// Deve ter pelo menos 8 caracteres, incluindo letra maiúscula, minúscula, número e caractere especial.
        /// </summary>
        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter entre {2} e {1} caracteres", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "A senha deve conter letras maiúsculas, minúsculas, números e caracteres especiais")]
        public string? NewPassword { get; set; }

        /// <summary>
        /// Confirmação da nova senha.
        /// Deve ser igual ao valor de <see cref="NewPassword"/>.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem")]
        public string? ConfirmPassword { get; set; }
    }
}
