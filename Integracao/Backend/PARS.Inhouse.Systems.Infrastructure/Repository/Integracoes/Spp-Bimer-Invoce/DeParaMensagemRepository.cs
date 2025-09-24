using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.SppBimmerInvoce;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;

namespace PARS.Inhouse.Systems.Infrastructure.Repository.Integracoes.SppBimerInvoce
{
    public class DeParaMensagemRepository: IDeParaMensagemRepository
    {
        private readonly Context _context;

        public DeParaMensagemRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<IntegracaoSppBimerDeParaMensagem>> GetAllAsync()
        {
            try
            {
                return await _context.IntegracaoSppBimerDeParaMensagem.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar registros.", ex);
            }
        }

        public async Task<IntegracaoSppBimerDeParaMensagem?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.IntegracaoSppBimerDeParaMensagem.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar registro por ID.", ex);
            }
        }

        public async Task AddAsync(IntegracaoSppBimerDeParaMensagem entity)
        {
            try
            {
                await _context.IntegracaoSppBimerDeParaMensagem.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar registro.", ex);
            }
        }

        public async Task UpdateAsync(IntegracaoSppBimerDeParaMensagem entity)
        {
            try
            {
                _context.IntegracaoSppBimerDeParaMensagem.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar registro.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _context.IntegracaoSppBimerDeParaMensagem.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar registro.", ex);
            }
        }

        public async Task<IntegracaoSppBimerDeParaMensagem?> ObterMensagemMapeadaAsync(string mensagemPadrao)
        {
            try
            {
                return await _context.IntegracaoSppBimerDeParaMensagem
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.MensagemPadrao == mensagemPadrao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar registro por ID.", ex);
            }
        }
    }
}
