// Application/Services/.cs
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Application.Services
{
    public interface ITipoTemplateService
    {
        Task<TipoTemplateResponseDto?> ObterPorIdTipoTemplateAsync(int id);
        Task<IEnumerable<TipoTemplateResponseDto>> ObterTodosTipoTemplatesAsync();
        Task<TipoTemplateRequestDto> NovoTipoTemplateAsync(TipoTemplateRequestDto dto);
        Task<TipoTemplateRequestDto?> AtualizarTipoTemplateAsync(int id, TipoTemplateRequestDto dto);
        Task<bool> DeletarTipoTemplateAsync(int id);
    }
}