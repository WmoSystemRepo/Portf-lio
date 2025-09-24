using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Templates.MongoDb
{
    /// <summary>
    /// Representa um template de planilha para importação no contexto da integração Adobe Hub.
    /// Este template define a estrutura esperada (colunas, cabeçalhos, etc) das planilhas.
    /// </summary>
    public class AdobeTemplante
    {
        /// <summary>
        /// Identificador único do documento no MongoDB.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// Nome do template, utilizado para identificação e upsert.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Quantidade total de colunas esperadas na planilha.
        /// </summary>
        public int QtdColunas { get; set; }

        /// <summary>
        /// Mapeamento das colunas esperadas: nome da coluna -> posição ou descrição.
        /// </summary>
        public Dictionary<string, string>? Colunas { get; set; }

        /// <summary>
        /// Número da linha que representa o cabeçalho (títulos das colunas).
        /// </summary>
        public int? LinhaCabecalho { get; set; }

        /// <summary>
        /// Nome da primeira coluna válida da planilha (ex: "A").
        /// </summary>
        public string? ColunaInicial { get; set; }

        /// <summary>
        /// Nome do arquivo de exemplo ou base para comparação.
        /// </summary>
        public string? ArquivoBase { get; set; }

        /// <summary>
        /// Observações ou descrição adicional sobre o template.
        /// </summary>
        public string? ObservacaoDescricao { get; set; }

        /// <summary>
        /// Lista de nomes de colunas obrigatórias para validação.
        /// </summary>
        public List<string>? ColunasObrigatorias { get; set; }

        /// <summary>
        /// Data de criação do template (UTC).
        /// </summary>
        public string? DataCriacao { get; set; }

        /// <summary>
        /// Data da última edição do template (UTC).
        /// </summary>
        public string? DataEdicao { get; set; }

        /// <summary>
        /// ✅ Objeto completo do tipo de template.
        /// </summary>
        public TipoTemplateRequestDto? TipoTemplante { get; set; }
    }
}
