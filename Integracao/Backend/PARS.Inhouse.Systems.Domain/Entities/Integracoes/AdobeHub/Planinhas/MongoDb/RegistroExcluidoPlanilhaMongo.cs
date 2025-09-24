using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.MongoDb
{
    public class RegistroExcluidoPlanilhaMongo
    {
        /// <summary>
        /// Identificador único da planilha importada (pode ser um GUID, hash ou chave externa).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Tipo da planilha (ex: "Clientes", "Produtos", "Faturas").
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do arquivo original importado pelo usuário.
        /// </summary>
        public string NomeArquivo { get; set; } = string.Empty;

        /// <summary>
        /// Data do upload da planilha, no formato string (pode seguir padrão ISO 8601).
        /// </summary>
        public string? DataUpload { get; set; }

        /// <summary>
        /// Lista de registros contidos na planilha.
        /// Cada item representa uma linha, mapeada como dicionário com chave-valor (coluna-valor).
        /// </summary>
        public List<Dictionary<string, string>>? Dados { get; set; }

        public TipoTemplateRequestDto? TipoTemplante { get; set; }
    }
}
