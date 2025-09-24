using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Templantes_Planilhas
{
    public interface ITemplatePlanilhaService
    {
        Task<List<TemplantesMongoDto>> ListarTemplatesAsync(CancellationToken cancellationToken);
        Task NovoTemplatesAsync(TemplantesMongoDto dto, CancellationToken cancellationToken);
        Task<bool> RemoverTemplatePorIdAsync(string templateId, CancellationToken cancellationToken);
    }
}
