using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.SqlServe
{
    /// <summary>
    /// Representa o histórico de importações de planilhas Excel do Adobe Hub no banco SQL Server.
    /// </summary>
    [Table("IntegracaoAdobeHubHistoricoImportacaoExecel")]
    public class IntegracaoAdobeHubHistoricoImportacaoExecel
    {
        /// <summary>
        /// Identificador único da importação (chave primária).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tipo do template associado à planilha (ex: VIP, GOV, EDU).
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do arquivo Excel importado.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string NomeArquivo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora em que a planilha foi importada.
        /// </summary>
        public DateTime DataUpload { get; set; }

        /// <summary>
        /// Nome do usuário que realizou a importação.
        /// </summary>
        public string? Usuario { get; set; }

        /// <summary>
        /// Número da versão atribuída à importação da planilha.
        /// </summary>
        public int Versao { get; set; }

        /// <summary>
        /// Conteúdo da planilha em formato JSON (linhas e colunas).
        /// </summary>
        [Column(TypeName = "nvarchar(max)")]
        public string ExcelJson { get; set; } = string.Empty;
    }
}