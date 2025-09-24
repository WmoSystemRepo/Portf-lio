using PARS.Inhouse.Systems.Domain.Entities.Integracoes.SppBimmerInvoce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce
{
    public interface IDeParaMensagemRepository
    {
        Task<List<IntegracaoSppBimerDeParaMensagem>> GetAllAsync();
        Task<IntegracaoSppBimerDeParaMensagem?> GetByIdAsync(int id);
        Task<IntegracaoSppBimerDeParaMensagem?> ObterMensagemMapeadaAsync(string mensagemPadrao);
        Task AddAsync(IntegracaoSppBimerDeParaMensagem entity);
        Task UpdateAsync(IntegracaoSppBimerDeParaMensagem entity);
        Task DeleteAsync(int id);
    }
}
