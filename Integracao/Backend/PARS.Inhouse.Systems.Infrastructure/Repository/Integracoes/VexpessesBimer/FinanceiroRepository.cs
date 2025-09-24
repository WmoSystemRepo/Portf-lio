using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using System.Globalization;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.VexpessesBimer
{
    public class FinanceiroRepository : IBimerRepositorio
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public FinanceiroRepository(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task RegistrarTitulosPendentesAsync(IntegracaoBimmerInsercaoPendentes pendente, CancellationToken cancellationToken)
        {
            try
            {
                await _context.IntegracaoBimmerInsercaoPendentes.AddAsync(pendente);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RemovePendenteByTitleIdAsync(int titleId, CancellationToken cancellationToken)
        {
            var pendentesExistentes = await _context.IntegracaoBimmerInsercaoPendentes.Where(x => x.IdResponse == titleId)
                                                  .ToListAsync();
            if (pendentesExistentes.Count != 0)
            {
                _context.IntegracaoBimmerInsercaoPendentes.RemoveRange(pendentesExistentes);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IReadOnlyList<IntegracaoBimmerInsercaoPendentes>> GetPendenciasAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var query = _context.IntegracaoBimmerInsercaoPendentes.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var formats = new[] { "dd/MM/yyyy" };
                    if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
                    {
                        query = query.Where(r =>
                            r.DataCadastro.Value.Date == searchDate.Date);
                    }
                    else
                    {
                        query = query.Where(r =>
                            r.IdResponse.ToString().Contains(search) ||
                            r.UserId.ToString().Contains(search) ||
                            r.Descricao.ToString().Contains(search) ||
                            r.Valor.ToString().Contains(search));
                    }
                }

                List<IntegracaoBimmerInsercaoPendentes> reports = await query
                    .OrderByDescending(r => r.DataCadastro)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                cancellationToken.ThrowIfCancellationRequested();
                var dto = _mapper.Map<IReadOnlyList<IntegracaoBimmerInsercaoPendentes>>(reports);

                return dto;
                //return await _context.IntegracaoBimmerInsercaoPendentes.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
