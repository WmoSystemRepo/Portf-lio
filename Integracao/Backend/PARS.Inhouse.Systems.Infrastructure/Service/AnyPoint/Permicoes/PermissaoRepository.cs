using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.PermissoesMenu;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.User;

namespace PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.Permicoes
{
    public class PermissaoRepository : IPermissaoRepository
    {
        private readonly Context _context;

        public PermissaoRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<AnyPointStorePermicoes>> GetAllPermissoesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.AnyPointStorePermicoes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AnyPointStorePermicoes?> RecuperarPermissaoPorIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _context.AnyPointStorePermicoes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AtualizarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(permissao);

            var existing = await RecuperarPermissaoPorIdAsync(permissao.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException("Permissão não encontrada!");

            cancellationToken.ThrowIfCancellationRequested();
            _context.AnyPointStorePermicoes.Update(permissao);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarPermissaoAsync(AnyPointStorePermicoes permissao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(permissao);

            cancellationToken.ThrowIfCancellationRequested();
            _context.AnyPointStorePermicoes.Remove(permissao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PermissaoUsuarioDto>> GetPermissaoUsuarioByIdAsync(int id, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT pm.Id, pm.IdPermissao, u.UserName as NomeUsuario, u.Id as IdUsuario 
                FROM AnyPointPermissaoUsuario pm 
                LEFT JOIN AspNetUsers u ON pm.IdUsuario = u.Id 
                WHERE pm.IdPermissao = @id";

            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Database
                .SqlQueryRaw<PermissaoUsuarioDto>(query, new SqlParameter("@id", id))
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetUsuariosForPermissionSelectByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var usersSelect = new List<UserDto>();
                var query = @"
                SELECT u.UserName as NomeUsuario, u.Id as IdUsuario
                FROM AspNetUsers u";

                cancellationToken.ThrowIfCancellationRequested();
                var users = await _context.Database
                    .SqlQueryRaw<UserDto>(query)
                    .ToListAsync();

                foreach (var item in users)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var validarSeJaTemPermissao = _context.AnyPointPermissaoUsuario.Any(x => x.IdUsuario == item.IdUsuario && x.IdPermissao == id);
                    if (!validarSeJaTemPermissao)
                    {
                        usersSelect.Add(item);
                    }
                }
                return usersSelect;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<RoleDto>> GetPermissaoRoleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var query = @"
                SELECT r.Id, r.Name FROM AnyPointPermissao p
                INNER JOIN AspNetRoles r ON p.IdRole = r.Id
                WHERE p.Id = @id";

            cancellationToken.ThrowIfCancellationRequested();
            return await _context.Database
                .SqlQueryRaw<RoleDto>(query, new SqlParameter("@id", id))
                .ToListAsync();
        }

        public async Task<bool> SetUserPermissionForUserAsync(string userId, int permissionId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(userId);

            var permissaoUsuario = new AnyPointPermissaoUsuario
            {
                IdPermissao = permissionId,
                IdUsuario = userId
            };

            cancellationToken.ThrowIfCancellationRequested();
            await _context.AnyPointPermissaoUsuario.AddAsync(permissaoUsuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetUserPermissionForUsersAsync(string[] userIds, int permissionId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(userIds);

            cancellationToken.ThrowIfCancellationRequested();
            var permissoesUsuarios = userIds
                .Where(userId => !string.IsNullOrWhiteSpace(userId))
                .Select(userId => new AnyPointPermissaoUsuario
                {
                    IdPermissao = permissionId,
                    IdUsuario = userId
                })
                .ToList();

            foreach (var item in permissoesUsuarios)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var validarSeJaTemPermissao = _context.AnyPointPermissaoUsuario.Any(x => x.IdUsuario == item.IdUsuario && x.IdPermissao == item.IdPermissao);
                if (validarSeJaTemPermissao == true) continue;

                await _context.AnyPointPermissaoUsuario.AddAsync(item);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task RemoveUserFromPermission(int permissionId, string userId, CancellationToken cancellationToken)
        {
            var userPermission = await _context.AnyPointPermissaoUsuario
                .FirstOrDefaultAsync(p => p.IdUsuario == userId && p.IdPermissao == permissionId);

            if (userPermission != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _context.AnyPointPermissaoUsuario.Remove(userPermission);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Registra uma nova permissão no banco de dados.
        /// </summary>
        /// <param name="permicoesDB">Objeto contendo os dados da permissão.</param>
        /// <param name="tokenCancelamento">Token de cancelamento.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        /// <exception cref="ArgumentNullException">Lançada se permicoesDB for nulo.</exception>
        /// <exception cref="OperationCanceledException">Lançada se o token for solicitado.</exception>
        public async Task RegistrarPermicoesAsync(AnyPointStorePermicoes permicoesDB, CancellationToken tokenCancelamento)
        {
            ArgumentNullException.ThrowIfNull(permicoesDB);
            tokenCancelamento.ThrowIfCancellationRequested();

            _context.AnyPointStorePermicoes.Add(permicoesDB);
            await _context.SaveChangesAsync(tokenCancelamento);
        }
    }

    public class PermissaoUsuarioDto
    {
        public int? Id { get; set; }
        public int? IdPermissao { get; set; }
        public string? NomeUsuario { get; set; }
        public string? IdUsuario { get; set; }
    }
}
