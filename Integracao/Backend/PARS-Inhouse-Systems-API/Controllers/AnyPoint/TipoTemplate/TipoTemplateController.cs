using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PARS.Inhouse.Systems.Application.Services;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.API.Controllers
{
    [Route("api/Tipo/Templante")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Sistema Any Point Store: APIs Tipo Templantes")]
    [Authorize]
    public class TipoTemplateController : ControllerBase
    {
        private readonly ITipoTemplateService _service;

        public TipoTemplateController(ITipoTemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosTipoTemplates()
        {
            try
            {
                var result = await _service.ObterTodosTipoTemplatesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar TiposTemplate: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdTipoTemplate(int id)
        {
            try
            {
                var result = await _service.ObterPorIdTipoTemplateAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar TipoTemplate Id={id}: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> NovoTipoTemplate([FromBody] TipoTemplateRequestDto dto)
        {
            try
            {
                var result = await _service.NovoTipoTemplateAsync(dto);
                return CreatedAtAction(nameof(ObterPorIdTipoTemplate), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar TipoTemplate: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTipoTemplate(int id, [FromBody] TipoTemplateRequestDto dto)
        {
            try
            {
                var result = await _service.AtualizarTipoTemplateAsync(id, dto);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar TipoTemplate Id={id}: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarTipoTemplate(int id)
        {
            try
            {
                var success = await _service.DeletarTipoTemplateAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir TipoTemplate Id={id}: {ex.Message}");
            }
        }
    }
}