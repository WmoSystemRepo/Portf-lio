namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO utilizado para solicitar o envio de e-mail de redefinição de senha.
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Endereço de e-mail do usuário que deseja redefinir a senha.
        /// O sistema utilizará este e-mail para envio do link de recuperação.
        /// </summary>
        public string? Email { get; set; }
    }
}
