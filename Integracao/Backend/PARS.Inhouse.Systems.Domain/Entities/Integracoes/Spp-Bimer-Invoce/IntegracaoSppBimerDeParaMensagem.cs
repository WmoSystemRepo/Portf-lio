using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.SppBimmerInvoce
{
    [Table("IntegracaoSppBimerDeParaMensagem")]
    public class IntegracaoSppBimerDeParaMensagem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MensagemPadrao { get; set; } = string.Empty;

        [Required]
        public string MensagemModificada { get; set; } = string.Empty;

        [Required]
        public bool Ativo { get; set; }

        [Required]
        public string UsuarioCadastro { get; set; } = string.Empty;

        public string? UsuarioEdicao { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        public DateTime? DataEdicao { get; set; }
    }
}