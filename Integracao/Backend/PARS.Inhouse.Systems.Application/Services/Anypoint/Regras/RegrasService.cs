namespace PARS.Inhouse.Systems.Application.Services.Regras
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint.Regras;
    using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.Regras;

    public class RegrasService : IRegrasService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegrasService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RegraDto>> ObterTodasRegrasAsync(CancellationToken cancellationToken)
        {
            return await _roleManager.Roles
                .Select(r => new RegraDto
                {
                    Id = r.Id,
                    Nome = r.Name
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<RegraDto?> ObterRegraPorIdAsync(string id, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return null;

            return new RegraDto
            {
                Id = role.Id,
                Nome = role.Name
            };
        }

        public async Task<bool> CriarRegraAsync(RegraDto dto, CancellationToken cancellationToken)
        {
            if (await _roleManager.RoleExistsAsync(dto.Nome))
                return false;

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.Nome));
            return result.Succeeded;
        }

        public async Task<bool> AtualizarRegraAsync(string id, RegraDto dto, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return false;

            role.Name = dto.Nome;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> ExcluirRegraAsync(string nome, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(nome);
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
    }
}

