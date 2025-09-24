namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User
{
    /// <summary>
    /// DTO utilizado para edição dos dados de um usuário no sistema.
    /// Inclui dados pessoais e permissões de acesso granular.
    /// </summary>
    public class UserEditDto
    {
        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Nome de usuário (login) do sistema.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Número de telefone do usuário para contato ou autenticação.
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Indica se o usuário pode visualizar informações no sistema.
        /// </summary>
        public bool PodeLer { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissão para criar ou editar registros.
        /// </summary>
        public bool PodeEscrever { get; set; }

        /// <summary>
        /// Indica se o usuário tem permissão para excluir registros.
        /// </summary>
        public bool PodeRemover { get; set; }

        /// <summary>
        /// Indica se o usuário pode acessar e visualizar configurações do sistema.
        /// </summary>
        public bool PodeVerConfiguracoes { get; set; }
    }
}
