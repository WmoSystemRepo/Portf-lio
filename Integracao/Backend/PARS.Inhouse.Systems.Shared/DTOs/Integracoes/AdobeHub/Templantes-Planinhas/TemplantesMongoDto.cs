using System.Text.Json.Serialization;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub
{
    /// <summary>
    /// DTO que representa a definição de um template (modelo) de importação/exportação armazenado no MongoDB.
    /// Utilizado para interpretar e mapear dados de arquivos dinâmicos.
    /// </summary>
    public class TemplantesMongoDto
    {
        /// <summary>
        /// Identificador único do template (gerado pelo MongoDB).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nome descritivo do template (ex: "Modelo de Produto", "Planilha Financeira").
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Quantidade total de colunas mapeadas no template.
        /// </summary>
        public int QtdColunas { get; set; }

        /// <summary>
        /// Dicionário com o mapeamento das colunas.
        /// Chave: nome da coluna original na planilha. Valor: nome interno ou amigável.
        /// </summary>
        public Dictionary<string, string>? Colunas { get; set; }

        /// <summary>
        /// Linha onde se encontra o cabeçalho da planilha.
        /// </summary>
        public int? LinhaCabecalho { get; set; }

        /// <summary>
        /// Coluna inicial do conteúdo, geralmente uma letra (ex: "A", "B").
        /// </summary>
        public string? ColunaInicial { get; set; }

        /// <summary>
        /// Caminho ou nome do arquivo base associado ao template (caso exista).
        /// </summary>
        public string? ArquivoBase { get; set; }

        /// <summary>
        /// Observações ou instruções adicionais sobre o uso do template.
        /// </summary>
        public string? ObservacaoDescricao { get; set; }

        /// <summary>
        /// Lista com os nomes das colunas obrigatórias que devem estar presentes no arquivo.
        /// </summary>
        public List<string>? ColunasObrigatorias { get; set; }

        /// <summary>
        /// Data de criação do template (formato string ou ISO 8601).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última modificação do template (formato string ou ISO 8601).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// Tipo do template (agora com objeto completo vindo do banco).
        /// </summary>
        public TipoTemplateRequestDto? TipoTemplate { get; set; }
    }
}
