using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;

namespace PARS.Inhouse.Systems.Domain.Extensions
{
    public static class LogErroMapper
    {
        public static IntegracaoVexpenssesBimmerLogErros ToSqlEntity(this IntegracaoVexpenssesBimmerLogErros source)
        {
            return new IntegracaoVexpenssesBimmerLogErros
            {
                DataHoraUtc = source.DataHoraUtc,
                Endpoint = source.Endpoint,
                Payload = source.Payload,
                Message = source.Message
            };
        }
    }
}
