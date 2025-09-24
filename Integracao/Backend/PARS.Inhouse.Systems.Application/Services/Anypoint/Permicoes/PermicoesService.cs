using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint.Permicoes
{
    public class PermicoesService : IPermicoesService
    {
        private readonly IPermissaoRepository _repositorio;

        public PermicoesService(IPermissaoRepository repository)
        {
            _repositorio = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<AnyPointStorePermicoes>> ObterTodasPermissaoAsync(CancellationToken cancellationToken)
        {
            var permissoes = await _repositorio.GetAllPermissoesAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            return permissoes.AsReadOnly();
        }

        public async Task<AnyPointStorePermicoes?> ObterPermissaoPorIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            return await _repositorio.RecuperarPermissaoPorIdAsync(id, cancellationToken);
        }

        public async Task AtualizarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(permissao, nameof(permissao));

            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.AtualizarPermissaoAsync(permissao, cancellationToken);
        }

        public async Task DeletaPermissaoAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(id));
            }

            var permissao = await _repositorio.RecuperarPermissaoPorIdAsync(id, cancellationToken);
            if (permissao == null)
            {
                throw new KeyNotFoundException($"Permissão com ID {id} não encontrada.");
            }

            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.DeletarPermissaoAsync(permissao, cancellationToken);
        }




        public async Task<IReadOnlyList<PermissaoUsuarioDto>> GetUsersLinkedToPermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

            cancellationToken.ThrowIfCancellationRequested();
            var users = await _repositorio.GetPermissaoUsuarioByIdAsync(permissionId, cancellationToken);
            return users.AsReadOnly();
        }

        public async Task<IReadOnlyList<UserDto>> GetUsersToPermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

            cancellationToken.ThrowIfCancellationRequested();
            var users = await _repositorio.GetUsuariosForPermissionSelectByIdAsync(permissionId, cancellationToken);
            return users.AsReadOnly();
        }

        public async Task<IReadOnlyList<RoleDto>> GetRolesLinkedToPermissionAsync(int permissionId, CancellationToken cancellationToken)
        {
            if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

            cancellationToken.ThrowIfCancellationRequested();
            var roles = await _repositorio.GetPermissaoRoleByIdAsync(permissionId, cancellationToken);
            return roles.AsReadOnly();
        }

        //public async Task<bool> AssignRoleToPermissionAsync(string roleId, int permissionId, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(roleId)) throw new ArgumentException("O ID da role não pode ser nulo ou vazio.", nameof(roleId));
        //    if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

        //    cancellationToken.ThrowIfCancellationRequested();
        //    return await _repositorioBimer.SetRoleOnPermissionAsync(roleId, permissionId, cancellationToken);
        //}

        //public async Task<bool> AssignPermissionToUserAsync(string userId, int permissionId, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("O ID do usuário não pode ser nulo ou vazio.", nameof(userId));
        //    if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

        //    cancellationToken.ThrowIfCancellationRequested();
        //    return await _repositorioBimer.SetUserPermissionForUserAsync(userId, permissionId, cancellationToken);
        //}

        public async Task<bool> AssignPermissionToUsersAsync(string[] userIds, int permissionId, CancellationToken cancellationToken)
        {
            if (userIds == null || userIds.Length == 0) throw new ArgumentException("A lista de usuários não pode estar vazia.", nameof(userIds));
            if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

            cancellationToken.ThrowIfCancellationRequested();
            return await _repositorio.SetUserPermissionForUsersAsync(userIds, permissionId, cancellationToken);
        }

        public async Task RemoveUserFromPermissionAsync(int permissionId, string userId, CancellationToken cancellationToken)
        {
            if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("O ID do usuário não pode ser nulo ou vazio.", nameof(userId));

            cancellationToken.ThrowIfCancellationRequested();
            await _repositorio.RemoveUserFromPermission(permissionId, userId, cancellationToken);
        }

        //public async Task RemoveRoleFromPermissionAsync(string roleId, int permissionId, CancellationToken cancellationToken)
        //{
        //    if (string.IsNullOrWhiteSpace(roleId)) throw new ArgumentException("O ID da role não pode ser nulo ou vazio.", nameof(roleId));
        //    if (permissionId <= 0) throw new ArgumentException("O ID da permissão deve ser maior que zero.", nameof(permissionId));

        //    cancellationToken.ThrowIfCancellationRequested();
        //    await _repositorioBimer.RemoveRoleFromPermission(roleId, permissionId, cancellationToken);
        //}

        /// <summary>
        /// Registra uma nova permissão no sistema.
        /// </summary>
        /// <param name="dto">Objeto contendo os dados da permissão a ser registrada.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de persistência no banco de dados.</returns>
        /// <exception cref="ArgumentNullException">Lançada quando o DTO fornecido é nulo.</exception>
        /// <exception cref="OperationCanceledException">Lançada se o token de cancelamento for solicitado.</exception>
        public async Task CriarPermicoesAsync(PermicoesDto dto, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dto);
            cancellationToken.ThrowIfCancellationRequested();

            var permissao = new AnyPointStorePermicoes
            {
                Nome = dto.Nome,
                DataCriacao = TentarConverterData(dto.DataCriacao, DateTime.UtcNow),
                DataEdicao = TentarConverterData(dto.DataEdicao, DateTime.UtcNow)
            };

            await _repositorio.RegistrarPermicoesAsync(permissao, cancellationToken);
        }


        /// <summary>
        /// Tenta converter uma string para <see cref="DateTime"/>; retorna um valor padrão se falhar.
        /// </summary>
        /// <param name="entrada">Data em formato de texto.</param>
        /// <param name="padrao">Valor de fallback caso falhe a conversão.</param>
        /// <returns>Data convertida ou o valor padrão informado.</returns>
        private static DateTime TentarConverterData(string? entrada, DateTime padrao)
        {
            return DateTime.TryParse(entrada, out var resultado) ? resultado : padrao;
        }
    }
}
