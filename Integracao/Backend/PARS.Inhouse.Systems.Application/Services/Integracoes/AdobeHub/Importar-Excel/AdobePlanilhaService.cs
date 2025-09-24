using MongoDB.Bson;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Importar_Excel;
using PARS.Inhouse.Systems.Application.Interfaces.Integracoes.AdobeHub.Templantes_Planilhas;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb;
using PARS.Inhouse.Systems.Infrastructure.Interfaces.Integracoes.AdobeHub;
using PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub;

namespace PARS.Inhouse.Systems.Application.Services.Integracoes.AdobeHub.Importar_Excel;

/// <summary>
/// Serviço central responsável por lidar com templates de planilhas e planilhas importadas (persistência no Mongo).
/// Implementa interfaces separadas para garantir coesão e testabilidade.
/// </summary>
public class AdobePlanilhaService : ITemplatePlanilhaService, IImportacaoPlanilhaService
{
    private readonly IAdobeTemplantesExcelMongoRepository _templatePlanilhaRepository;

    public AdobePlanilhaService(IAdobeTemplantesExcelMongoRepository templatePlanilhaRepository)
    {
        _templatePlanilhaRepository = templatePlanilhaRepository;
    }

    #region 🔷 Templates

    public async Task<List<TemplantesMongoDto>> ListarTemplatesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var templates = await _templatePlanilhaRepository.ListarTodosAsync(cancellationToken);

        return templates.Select(t => new TemplantesMongoDto
        {
            Id = t.Id,
            Nome = t.Nome,
            QtdColunas = t.QtdColunas,
            Colunas = t.Colunas,
            LinhaCabecalho = t.LinhaCabecalho,
            ColunaInicial = t.ColunaInicial,
            ArquivoBase = t.ArquivoBase,
            ObservacaoDescricao = t.ObservacaoDescricao,
            ColunasObrigatorias = t.ColunasObrigatorias,
            DataCriacao = t.DataCriacao,
            DataEdicao = t.DataEdicao,
            TipoTemplate = t.TipoTemplante
        }).ToList();
    }

    public async Task NovoTemplatesAsync(TemplantesMongoDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _templatePlanilhaRepository.NovoTemplante(dto, cancellationToken);
    }

    public async Task<bool> RemoverTemplatePorIdAsync(string templateId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(templateId))
            throw new ArgumentException("ID do template não pode ser vazio.", nameof(templateId));

        cancellationToken.ThrowIfCancellationRequested();
        return await _templatePlanilhaRepository.RemoverTemplatePorIdAsync(templateId, cancellationToken);
    }

    #endregion

    #region 📥 Planilhas Importadas

    public async Task<List<PlaninhasImportadosDto>> ListaPlaninhasImportadasAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entidades = await _templatePlanilhaRepository.ListarPlanilhasImportadasAsync(cancellationToken);

        return entidades.Select(e => new PlaninhasImportadosDto
        {
            Id = e.Id,
            NomeArquivo = e.NomeArquivo,
            DataUpload = e.DataUpload,
            Dados = e.Dados,
            Versao = e.Versao,
            Usuario = e.Usuario,
            Template = e.Template
        }).ToList();
    }

    public async Task<PlaninhasImportadosDto?> BuscarPlanilhaPorIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entidade = await _templatePlanilhaRepository.BuscarPlanilhaPorIdAsync(id, cancellationToken);
        if (entidade == null) return null;

        return new PlaninhasImportadosDto
        {
            Id = entidade.Id,
            NomeArquivo = entidade.NomeArquivo,
            DataUpload = entidade.DataUpload,
            Versao = entidade.Versao,
            Usuario = entidade.Usuario,
            Dados = entidade.Dados,
            Template = entidade.Template
        };
    }

    public async Task<bool> RemoverPlanilhaPorIdAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("ID da planilha não pode ser vazio.", nameof(id));

        cancellationToken.ThrowIfCancellationRequested();
        return await _templatePlanilhaRepository.RemoverPlanilhaPorIdAsync(id, cancellationToken);
    }

    public async Task<string> SalvarProdutoAsync(string planilhaId, SalvarProdutoDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(planilhaId))
            throw new ArgumentException("planilhaId inválido.", nameof(planilhaId));

        if (dto is null || dto.Produto is null || dto.Produto.Count == 0)
            throw new ArgumentException("Produto inválido ou ausente.", nameof(dto));

        await _templatePlanilhaRepository.AdicionarProdutoNoFinalAsync(planilhaId, dto.Produto, cancellationToken);

        return "inserted";
    }

    public async Task SalvarPlanilhaImportadaAsync(PlaninhasImportadosDto dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entidade = new PlanilhasImportadasMongo
        {
            Id = ObjectId.GenerateNewId().ToString(),
            NomeArquivo = dto.NomeArquivo,
            DataUpload = dto.DataUpload ?? DateTime.UtcNow.ToString("o"),
            Versao = dto.Versao,
            Usuario = dto.Usuario,
            Dados = dto.Dados ?? new List<Dictionary<string, string>>(),
            Template = dto.Template
        };

        await _templatePlanilhaRepository.SalvarPlanilhaImportadaAsync(entidade, cancellationToken);
    }

    #endregion
}
