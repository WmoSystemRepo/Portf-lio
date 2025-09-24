using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.AdobeHub
{
    public class ExclusaoTemplateDto
    {
        /// <summary>
        /// Justificativa da exclusão fornecida pelo usuário.
        /// </summary>
        public string Justificativa { get; set; } = string.Empty;

        /// <summary>
        /// Lista de pendências que foram excluídas.
        /// </summary>
        public List<TemplantesMongoDto>? RegistrosExcluidos { get; set; } = new();

        /// <summary>
        /// Nome ou identificador do usuário que executou a exclusão.
        /// </summary>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que a exclusão foi realizada.
        /// </summary>
        public DateTime DataHora { get; set; }
    }
}
