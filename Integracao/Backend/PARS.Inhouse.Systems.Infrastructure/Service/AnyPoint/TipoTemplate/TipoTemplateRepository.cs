using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Infrastructure.Service.AnyPoint.TipoTemplate
{
    public class TipoTemplateRepository : ITipoTemplateRepository
    {
        private readonly Context _context;

        public TipoTemplateRepository(Context context)
        {
            _context = context;
        }

        public async Task<AnyPointStoreTipoTemplate?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.AnyPointStoreTiposTemplate.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao buscar TipoTemplate com Id={id}", ex);
            }
        }

        public async Task<IEnumerable<AnyPointStoreTipoTemplate>> GetAllAsync()
        {
            try
            {
                return await _context.AnyPointStoreTiposTemplate
                                        .AsNoTracking()
                                        .Include(x => x.Integracao)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar TiposTemplate", ex);
            }
        }

        public async Task AddAsync(AnyPointStoreTipoTemplate entity)
        {
            try
            {
                await _context.AnyPointStoreTiposTemplate.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao adicionar TipoTemplate", ex);
            }
        }

        public async Task UpdateAsync(AnyPointStoreTipoTemplate entity)
        {
            try
            {
                _context.AnyPointStoreTiposTemplate.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao atualizar TipoTemplate Id={entity.Id}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.AnyPointStoreTiposTemplate.FindAsync(id);
                if (entity != null)
                {
                    _context.AnyPointStoreTiposTemplate.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao excluir TipoTemplate Id={id}", ex);
            }
        }
    }
}