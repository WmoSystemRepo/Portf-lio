using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.SppBimmerInvoce;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.SppBimerInvoce;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.SppBimerInvoce;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.SppBimerInvoce
{
    public class DeParaMensagemService: IDeParaMensagemService
    {
        private readonly IDeParaMensagemRepository _repository;


        public DeParaMensagemService(IDeParaMensagemRepository repository)
        {
            _repository = repository;
        }



        public async Task<IEnumerable<DeParaMensagemDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Select(e => MapToDto(e));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os registros.", ex);
            }
        }


        public async Task<DeParaMensagemDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return entity == null ? null : MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter registro por ID.", ex);
            }
        }


        public async Task AddAsync(DeParaMensagemDto dto)
        {
            try
            {
                var entity = MapToEntity(dto);
                entity.DataCriacao = DateTime.UtcNow;
                await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar registro.", ex);
            }
        }


        public async Task UpdateAsync(int id, DeParaMensagemDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) return;


                entity.MensagemPadrao = dto.MensagemPadrao;
                entity.MensagemModificada = dto.MensagemModificada;
                entity.Ativo = dto.Ativo;
                entity.UsuarioEdicao = dto.UsuarioEdicao;
                entity.DataEdicao = DateTime.UtcNow;


                await _repository.UpdateAsync(entity);
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
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar registro.", ex);
            }
        }


        private static DeParaMensagemDto MapToDto(IntegracaoSppBimerDeParaMensagem entity)
        {
            return new DeParaMensagemDto
            {
                Id = entity.Id,
                MensagemPadrao = entity.MensagemPadrao,
                MensagemModificada = entity.MensagemModificada,
                Ativo = entity.Ativo,
                UsuarioCadastro = entity.UsuarioCadastro,
                UsuarioEdicao = entity.UsuarioEdicao,
                DataCriacao = entity.DataCriacao,
                DataEdicao = entity.DataEdicao
            };
        }


        private static IntegracaoSppBimerDeParaMensagem MapToEntity(DeParaMensagemDto dto)
        {
            return new IntegracaoSppBimerDeParaMensagem
            {
                MensagemPadrao = dto.MensagemPadrao,
                MensagemModificada = dto.MensagemModificada,
                Ativo = dto.Ativo,
                UsuarioCadastro = dto.UsuarioCadastro,
                UsuarioEdicao = dto.UsuarioEdicao,
                DataCriacao = dto.DataCriacao,
                DataEdicao = dto.DataEdicao
            };
        }

        public async Task<DeParaMensagemDto?> ObterDeparaPorMensagemPadraoAsync(string mensagemPadrao)
        {
            try
            {
                var entity = await _repository.ObterMensagemMapeadaAsync(mensagemPadrao);
                return entity == null ? null : MapToDto(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar registro.", ex);
            }
        }
    }
}
