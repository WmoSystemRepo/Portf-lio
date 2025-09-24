using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARS.Inhouse.Systems.Domain.Entities.AnyPoint.TipoTemplate
{
    /// <summary>
    /// Representa um tipo de template no AnyPoint Store.
    /// Contém informações de identificação e referência para integração associada.
    /// </summary>
    public class AnyPointStoreTipoTemplate
    {
        /// <summary>
        /// Identificador único do tipo de template.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do template.
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string NomeCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Sigla identificadora do template.
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string Sigla { get; set; } = string.Empty;

        /// <summary>
        /// Nome Abreviado do template.
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string NomeAbreviado { get; set; } = string.Empty;

        /// <summary>
        /// Identificador da integração associada (chave estrangeira).
        /// </summary>
        [ForeignKey(nameof(Integracao))]
        public int IntegracaoId { get; set; }

        /// <summary>
        /// Entidade de integração associada.
        /// </summary>
        public AnyPointStoreGestaoIntegracao? Integracao { get; set; } = null!;
    }
}