using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.CadastroIntegracao;

namespace PARS.Inhouse.Systems.Application.Services.Anypoint
{
    public class AnyPointCadastroIntegracaoService : IAnyPointCadastroIntegracaoService
    {
        private readonly IAnyPointCadastroIntegracaoRepository _repository;

        public AnyPointCadastroIntegracaoService(IAnyPointCadastroIntegracaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GestaoIntegracoesDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var integracoes = await _repository.GetAllAsync(cancellationToken);
            return integracoes.Select(i => new GestaoIntegracoesDto
            {
                Id = i.Id,
                Nome = i.Nome,
                ProjetoOrigem = i.ProjetoOrigem,
                ProjetoDestino = i.ProjetoDestino,
                DataCriacao = i.DataCriacao,
                DataEdicao = i.DataEdicao
            });
        }

        public async Task<GestaoIntegracoesDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var integracao = await _repository.GetByIdAsync(id, cancellationToken);
            if (integracao == null) return null;
            return new GestaoIntegracoesDto
            {
                Id = integracao.Id,
                Nome = integracao.Nome,
                ProjetoOrigem = integracao.ProjetoOrigem,
                ProjetoDestino = integracao.ProjetoDestino,
                DataCriacao = integracao.DataCriacao,
                DataEdicao = integracao.DataEdicao
            };
        }

        public async Task AddAsync(GestaoIntegracoesDto dto, CancellationToken cancellationToken)
        {
            var integracao = new AnyPointStoreGestaoIntegracao
            {
                Nome = dto.Nome,
                ProjetoOrigem = dto.ProjetoOrigem,
                ProjetoDestino = dto.ProjetoDestino,
                DataCriacao = dto.DataCriacao,
                DataEdicao = dto.DataEdicao
            };
            await _repository.AddAsync(integracao, cancellationToken);
        }

        public async Task UpdateAsync(GestaoIntegracoesDto dto, CancellationToken cancellationToken)
        {
            var integracao = await _repository.GetByIdAsync(dto.Id, cancellationToken);
            if (integracao == null) return;

            integracao.Nome = dto.Nome;
            integracao.ProjetoOrigem = dto.ProjetoOrigem;
            integracao.ProjetoDestino = dto.ProjetoDestino;
            integracao.DataCriacao = dto.DataCriacao;
            integracao.DataEdicao = dto.DataEdicao;

            await _repository.UpdateAsync(integracao, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}
