using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes
{
    public class AnyPointPermissaoUsuario
    {
        [Key]
        public int Id { get; set; }
        public string? IdUsuario { get; set; }

        [NotMapped]
        public string? NomeUsuario { get; set; }

        [ForeignKey("AnyPointPermissao")]
        public int IdPermissao { get; set; }
    }
}
