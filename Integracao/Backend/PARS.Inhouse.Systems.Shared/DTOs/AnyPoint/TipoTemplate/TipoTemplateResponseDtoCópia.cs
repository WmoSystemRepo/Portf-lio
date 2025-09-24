using PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.CadastroIntegracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Shared.DTOs.AnyPoint.TipoTemplate
{
    public class TipoTemplateResponseDto
    {
        public int? Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public int IntegracaoId { get; set; }
        public string NomeAbreviado { get; set; } = string.Empty;

        public GestaoIntegracoesDto? Integracao { get; set; } = null!;

    }
}