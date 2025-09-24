using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint
{
    public class EndpointPermissionService : IEndpointPermissionService
    {
        private readonly Context _context;

        public EndpointPermissionService(Context context)
        {
            _context = context;
        }

        public async Task<bool> HasPermissionAsync(string userId, string acao)
        {
            var permission = await _context.Users
                .FirstOrDefaultAsync(uep => uep.Id == userId);

            if (permission == null)
            {
                return false;
            }

            return acao.ToLower() switch
            {
                "read" => permission.PodeLer,
                "write" => permission.PodeEscrever,
                "delete" => permission.PodeRemover,
                _ => false,
            };
        }

        public async Task<bool> HasPermissionAsync(string userId, int endpointId, string acao)
        {
            var permission = await _context.Users
                .FirstOrDefaultAsync(uep => uep.Id == userId);

            if (permission == null)
            {
                return false;
            }

            return acao.ToLower() switch
            {
                "read" => permission.PodeLer,
                "write" => permission.PodeEscrever,
                "delete" => permission.PodeRemover,
                _ => false,
            };
        }
    }
}
