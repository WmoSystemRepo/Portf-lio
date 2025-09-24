using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe;

namespace PARS.Inhouse.Systems.Infrastructure.Repositories.Integracoes.AdobeHub.Calculo_Preco_Revenda_Adobe
{
    public class PrecoRevendaRepository : IPrecoRevendaRepository
    {
        private readonly SPPContext _context;

        public PrecoRevendaRepository(SPPContext context)
        {
            _context = context;
        }

        public async Task<IndicePrecoRevendaDto?> ObterIndiceAsync(CancellationToken cancellationToken)
        {
            var entidade = await _context.TabelaPrecosIndicesAdobe
                .AsNoTracking()
                .Where(x => x.Ativo)
                .OrderByDescending(x => x.DataCriacao)
                .FirstOrDefaultAsync(cancellationToken);

            if (entidade == null) return null;

            return new IndicePrecoRevendaDto
            {
                CustoOperacional = entidade.CustoOperacional,
                PIS = entidade.PIS,
                COFINS = entidade.COFINS,
                ICMS = entidade.ICMS,
                ISS = entidade.ISS,
                Marketing = entidade.Marketing,
                Outros = entidade.Outros,
                ProdNivel1 = entidade.ProdNivel1,
                OutrosProd = entidade.OutrosProd
            };
        }
    }
}
