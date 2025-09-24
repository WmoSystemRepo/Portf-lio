using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer
{
    public interface IIntegracaoVexpensesBimmerLogErroRepository
    {
        Task SaveAsync(IntegracaoVexpenssesBimmerLogErros log, CancellationToken ct = default);
    }
}