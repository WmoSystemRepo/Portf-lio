using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("TabelaPrecosIndicesAdobe")]
public class TabelaPrecosIndicesAdobe
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal CustoOperacional { get; set; }

    public decimal PIS { get; set; }
    public decimal COFINS { get; set; }
    public decimal ICMS { get; set; }
    public decimal ISS { get; set; }
    public decimal Marketing { get; set; }
    public decimal Outros { get; set; }

    public decimal MargemMinima { get; set; }
    public decimal ProdNivel1 { get; set; }
    public decimal OutrosProd { get; set; }

    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
