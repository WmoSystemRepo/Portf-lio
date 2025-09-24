using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoce
{
    public interface IDeParaMensagemService
    {
        Task<IEnumerable<DeParaMensagemDto>> GetAllAsync();
        Task<DeParaMensagemDto?> GetByIdAsync(int id);
        Task<DeParaMensagemDto?> ObterDeparaPorMensagemPadraoAsync(string mensagemPadrao);
        Task AddAsync(DeParaMensagemDto dto);
        Task UpdateAsync(int id, DeParaMensagemDto dto);
        Task DeleteAsync(int id);
    }
}
