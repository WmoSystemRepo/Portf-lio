using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.CadastroIntegracao;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Application.Services
{
    public class TipoTemplateService : ITipoTemplateService
    {
        private readonly ITipoTemplateRepository _repository;

        public TipoTemplateService(ITipoTemplateRepository repository)
        {
            _repository = repository;
        }

        public async Task<TipoTemplateResponseDto?> ObterPorIdTipoTemplateAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return entity == null ? null : MapToDtoReponse(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao buscar TipoTemplate com Id={id}", ex);
            }
        }

        public async Task<IEnumerable<TipoTemplateResponseDto>> ObterTodosTipoTemplatesAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Select(MapToDtoReponse);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar TiposTemplate", ex);
            }
        }

        public async Task<TipoTemplateRequestDto> NovoTipoTemplateAsync(TipoTemplateRequestDto dto)
        {
            try
            {
                var entity = new AnyPointStoreTipoTemplate
                {
                    NomeCompleto = dto.NomeCompleto,
                    Sigla = dto.Sigla,
                    IntegracaoId = dto.IntegracaoId,
                    NomeAbreviado = dto.NomeAbreviado
                };

                await _repository.AddAsync(entity);
                return MapToDtoRequest(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar TipoTemplate", ex);
            }
        }

        public async Task<TipoTemplateRequestDto?> AtualizarTipoTemplateAsync(int id, TipoTemplateRequestDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return null;

                entity.NomeCompleto = dto.NomeCompleto;
                entity.Sigla = dto.Sigla;
                entity.IntegracaoId = dto.IntegracaoId;
                entity.NomeAbreviado = dto.NomeAbreviado;

                await _repository.UpdateAsync(entity);
                return MapToDtoRequest(entity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao atualizar TipoTemplate Id={id}", ex);
            }
        }

        public async Task<bool> DeletarTipoTemplateAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return false;

                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao excluir TipoTemplate Id={id}", ex);
            }
        }

        private TipoTemplateResponseDto MapToDtoReponse(AnyPointStoreTipoTemplate entity)
        {
            return new TipoTemplateResponseDto
            {
                Id = entity.Id,
                NomeCompleto = entity.NomeCompleto,
                Sigla = entity.Sigla,
                NomeAbreviado = entity.NomeAbreviado,
                IntegracaoId = entity.IntegracaoId,
                Integracao = entity.Integracao == null ? null : new GestaoIntegracoesDto
                {
                    Id = entity.Integracao.Id,
                    Nome = entity.Integracao.Nome,
                    ProjetoOrigem = entity.Integracao.ProjetoOrigem,
                    ProjetoDestino = entity.Integracao.ProjetoDestino,
                    DataCriacao = entity.Integracao.DataCriacao,
                    DataEdicao = entity.Integracao.DataEdicao
                }
            };
        }

        private TipoTemplateRequestDto MapToDtoRequest(AnyPointStoreTipoTemplate entity)
        {
            return new TipoTemplateRequestDto
            {
                Id = entity.Id,
                NomeCompleto = entity.NomeCompleto,
                Sigla = entity.Sigla,
                NomeAbreviado = entity.NomeAbreviado,
                IntegracaoId = entity.IntegracaoId
            };
        }
    }
}