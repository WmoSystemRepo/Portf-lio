using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.TipoTemplate
{
    public interface ITipoTemplateRepository
    {
        Task<AnyPointStoreTipoTemplate?> GetByIdAsync(int id);
        Task<IEnumerable<AnyPointStoreTipoTemplate>> GetAllAsync();
        Task AddAsync(AnyPointStoreTipoTemplate entity);
        Task UpdateAsync(AnyPointStoreTipoTemplate entity);
        Task DeleteAsync(int id);
    }
}