using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.Vexpenses_Bimmer;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.GestaoMapeamento;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.VexpenssesBimer
{
    public class GestaoMpeamentosService : IGestaoMepeamentoComposService
    {
        private readonly IGestaoMepeamentoComposRepository _repository;

        public GestaoMpeamentosService(IGestaoMepeamentoComposRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AnyPointDeparas>> ListarMapeamentoCamposAsync(CancellationToken cancellationToken)
        {
            return await _repository.ListarMapeamentoCamposAsync(cancellationToken);
        }

        public async Task<AnyPointDeparas?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.ObterPorIdAsync(id, cancellationToken);
        }

        public async Task NovoRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            await _repository.NovoRegistroAsync(entity, cancellationToken);
        }

        public async Task EditarRegistroAsync(AnyPointDeparas entity, CancellationToken cancellationToken)
        {
            await _repository.EditarRegistroAsync(entity, cancellationToken);
        }

        public async Task ExcluirRegistroAsync(int id, CancellationToken cancellationToken)
        {
            await _repository.ExcluirRegistroAsync(id, cancellationToken);
        }

        public async Task MapeamentoIntegracaoServicoRegistrarAsync(List<MapeamentoIntegracaoDto> integracoes, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (integracoes == null || integracoes.Count == 0)
                    throw new ArgumentException("A lista de integrações não pode ser nula ou vazia.");

                await _repository.MapeamentoIntegracaoRepositorioRegistrarAsync(integracoes, cancellationToken);

                Console.WriteLine($"✅ {integracoes.Count} integrações registradas com sucesso.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("⏳ Operação cancelada pelo cliente.");
                throw;
            }
            catch (ArgumentException argEx)
            {
                Console.WriteLine($"⚠️ Erro de entrada: {argEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao registrar integrações: {ex.Message}");
                throw new ApplicationException("Erro ao registrar as integrações do mapeamentoIntregracao.", ex);
            }
        }

        public async Task<AnyPointStoreMapeamentoIntegracao> MapeamentoIntegracaoServicoBuscaPorIdReferenciaAsync(int mapeamentoId, int integracaoId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var mapeamentoIntegracao = await _repository.MapeamentoIntegracaoRepositorioBuscarPorIdReferenciaAsync(mapeamentoId, integracaoId, cancellationToken);

                if (mapeamentoIntegracao is null)
                {
                    var msg = $"❗ Integração não encontrada. ID: {integracaoId}";
                    Console.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }

                return mapeamentoIntegracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuIntegracao ID: {integracaoId}");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"⚠️ {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                var errorMessage = $"❌ Erro inesperado ao buscar a MenuIntegração com ID {integracaoId}.";
                Console.WriteLine($"{errorMessage} Detalhes: {ex.Message}");
                throw new ApplicationException(errorMessage, ex);
            }
        }

        public async Task<List<AnyPointStoreMapeamentoIntegracao>> MapeamentoIntegracaoServicoBuscarPorIdMapeamentoAsync(int idMapeamento, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var mapeamentoIntregracao = await _repository.MapeamentoIntegracaoRepositorioBuscarPorIdMapeamentoAsync(idMapeamento, cancellationToken);

                if (mapeamentoIntregracao == null || !mapeamentoIntregracao.Any())
                    throw new KeyNotFoundException($"❗ Nenhuma integração encontrada para o Menu com ID {idMapeamento}.");

                return mapeamentoIntregracao;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"⏳ Operação cancelada para MenuId {idMapeamento}");
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw; // já está tratada acima com mensagem clara
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro inesperado ao buscar integrações do Menu ID {idMapeamento}: {ex.Message}");
                throw new ApplicationException("Erro inesperado ao buscar integrações do mapeamentoIntregracao.", ex);
            }
        }

        public async Task MapeamentoIntegracaoServicoDeletarAsync(AnyPointStoreMapeamentoIntegracao mapeamentoIntegracao, CancellationToken cancellationToken)
        {
            var idMapeamentotintegracao = mapeamentoIntegracao?.Id ?? 0;

            if (mapeamentoIntegracao == null)
            {
                throw new ArgumentNullException(nameof(mapeamentoIntegracao), "O objeto menu não pode ser nulo.");
            }

            if (idMapeamentotintegracao <= 0)
            {
                throw new ArgumentException("ID inválido para exclusão da integração.");
            }

            try
            {
                await _repository.MapeamentoIntegracaoRepositorioDeletarAsync(mapeamentoIntegracao, cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Erro ao tentar excluir a integração do menu no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao excluir a integração com ID {idMapeamentotintegracao}.", ex);
            }
        }
    }
}
