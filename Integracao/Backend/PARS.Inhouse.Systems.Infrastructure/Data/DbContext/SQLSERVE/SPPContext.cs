using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARS.Inhouse.Systems.Domain.Entities.Spp;

namespace PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE.SPP
{
    public sealed class ConstantesPedidoRow
    {
        public int ConstanteId { get; set; }

        public string? EmailCotacaoDolar { get; set; }
        public string? EmailFollowUpUsuarioFinal { get; set; }
        public string? EmailFollowUpSubscription { get; set; }
        public string? EmailFollowUpETLA { get; set; }
        public string? EmailAssessoriaTecnica { get; set; }

        public int? HoraLimiteDolar { get; set; }
        public int? MinutoLimiteDolar { get; set; }

        public string? EmailErroDolar { get; set; }
        public double? TotalImpostos { get; set; }

        public string? EmailSpyClienteGreen { get; set; }
        public int? EstoqueFaTNFm { get; set; }
        public int? EstoqueFatNFs { get; set; }

        public string? PWSusaDolarGarantido { get; set; }
        public string? TiinoAtivo { get; set; }
        public string? VerificaRevendaContrato { get; set; }

        public string MetodoMargemAdobe { get; set; } = "N";
    }

    public sealed class IndicesAdobeRow
    {
        public decimal PIS { get; set; }
        public decimal COFINS { get; set; }
        public decimal ISS { get; set; }
        public decimal CustoOperacional { get; set; }
        public decimal ProdNivel1 { get; set; }
        public decimal OutrosProd { get; set; }
        public decimal? MargemMinima { get; set; }
    }

    public sealed class TabelaPrecosPublicacoes
    {
        public int Id { get; set; }
        public int FabricanteId { get; set; }
        public string Segmento { get; set; } = string.Empty;
        public DateTime DataInicioValidade { get; set; }
        public DateTime? DataFimValidade { get; set; }
    }


    public class SPPContext : IdentityDbContext<ApplicationUserSPP>
    {
        public SPPContext(DbContextOptions<SPPContext> options)
            : base(options)
        {
        }

        // Tabelas reais com chaves
        public DbSet<TabelaPrecosIndicesAdobe> TabelaPrecosIndicesAdobe { get; set; } = default!;
        public DbSet<TabelaPrecosPublicacoes> TabelaPrecosPublicacoes { get; set; } = default!;

        // Views/Consultas (sem chave primária)
        public DbSet<ConstantesPedidoRow> Q_ConstantesPedido => Set<ConstantesPedidoRow>();
        public DbSet<IndicesAdobeRow> Q_IndicesAdobe => Set<IndicesAdobeRow>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TabelaPrecosIndicesAdobe>()
                .Property(p => p.CustoOperacional)
                .HasPrecision(5, 2);

            // Views sem chave
            modelBuilder.Entity<ConstantesPedidoRow>()
                .HasNoKey()
                .ToView(null);

            modelBuilder.Entity<IndicesAdobeRow>()
                .HasNoKey()
                .ToView(null);

            // Novo mapeamento para TabelaPrecosPublicacoes
            modelBuilder.Entity<TabelaPrecosPublicacoes>(entity =>
            {
                entity.ToTable("TabelaPrecosPublicacoes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Segmento).HasMaxLength(100);
            });
        }
    }
}
