using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.AutorizacaoEndpoints;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoIntegracao;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.GestaoMapeamentosDepara;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Menu;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Permicoes;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.TipoTemplate;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.User;
using PARS.Inhouse.Systems.Domain.Entities.AnyPoint.Usuario;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.AdobeHub.Planinhas.SqlServe;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Bacen.LogErros.MongoDb;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.Spp_Bimer_Invoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.SppBimmerInvoce;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.IntegracaoBimmerVexpense;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros;
using PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.Vexpense;
using System.Text.Json;

namespace PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<IntegracaoVexpenseTitulosRelatoriosStatus> IntegracaoVexpenseTitulosRelatoriosStatus { get; set; } = default!;

        // Mapeamento de campos (Depara)
        public DbSet<AnyPointDeparas> AnyPointDeparas { get; set; } = default!;
        public DbSet<AnyPointStoreMapeamentoIntegracao> AnyPointStoreMapeamentoIntegracao { get; set; }

        // Menus
        public DbSet<AnyPointStoreMenu> AnyPointStoreMenu { get; set; }
        public DbSet<AnyPointStoreMenuUsuario> AnyPointStoreMenuUsuario { get; set; }
        public DbSet<AnyPointStoreMenuIntegracao> AnyPointStoreMenuIntegracao { get; set; }
        public DbSet<AnyPointStoreMenuRegra> AnyPointStoreMenuRegra { get; set; }

        // Gestão de Integração
        public DbSet<AnyPointStoreGestaoIntegracao> AnyPointStoreGestaoIntegracao { get; set; } = default!;
        public DbSet<AnyPointStorePermicoes> AnyPointStorePermicoes { get; set; }

        // Usuario
        public DbSet<AnyPointStoreUsuarioRegra> AnyPointStoreUsuarioRegra { get; set; }
        public DbSet<AnyPointStoreUsuarioIntegracao> AnyPointStoreUsuarioIntegracao { get; set; }
        public DbSet<AnyPointStoreUsuarioPermissao> AnyPointStoreUsuarioPermissao { get; set; }

        // Integração Vexpenses com Bimer
        public DbSet<IntegracaoVexpenssesBimmerLogErros> IntegracaoVexpenssesBimmerLogErros { get; set; }

        // Integração Adobe Hub
        public DbSet<IntegracaoAdobeHubHistoricoImportacaoExecel> IntegracaoAdobeHubHistoricoImportacaoExecel { get; set; }
        public DbSet<IntegracaoAdobeHubLogsErros> IntegracaoAdobeHubLogsErros { get; set; }

        // Permissões
        public DbSet<AnyPointPermissaoUsuario> AnyPointPermissaoUsuario { get; set; }
        public DbSet<AnyPointUserEndpointPermission> AnyPointUserEndpointPermission { get; set; }

        // Integração Bimmer
        public DbSet<IntegracaoBimmerHistoricoErrosInsercoes> IntegracaoBimmerHistoricoErrosInsercoes { get; set; }
        public DbSet<IntegracaoBimmerInsercaoPendentes> IntegracaoBimmerInsercaoPendentes { get; set; }
        public DbSet<IntegracaoBimmerInsertOK> IntegracaoBimmerInsertOK { get; set; }

        // Integração Bacen
        public DbSet<IntegracaoBacenCotacaoMoeda> IntegracaoBacenCotacaoMoeda { get; set; }
        public DbSet<IntegracaoBacenLogErros> IntegracaoBacenLogErros { get; set; }

        // Integração SPP-Bimer-Invoce
        public DbSet<IntegracaoSppBimerInvoce> IntegracaoSppBimerInvoce { get; set; }
        public DbSet<IntegracaoSppBimerInvoceItens> IntegracaoSppBimerInvoceItens { get; set; }
        public DbSet<IntegracaoSppBimerInvoceResumo> IntegracaoSppBimerInvoceResumo { get; set; }
        public DbSet<IntegracaoSppBimerDeParaMensagem> IntegracaoSppBimerDeParaMensagem { get; set; }
        public DbSet<IntegracaoSppBimerInvoceLogErros> IntegracaoSppBimerInvoceLogErros { get; set; }

        // AnyPoint Store Tipos Template
        public DbSet<AnyPointStoreTipoTemplate> AnyPointStoreTiposTemplate { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var stringListConverter = new ValueConverter<ICollection<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions?)null)!);

            modelBuilder.Entity<IntegracaoSppBimerInvoce>()
                .HasKey(e => new { e.CdEmpresa, e.NumeroDocumento, e.NumeroDaNFSe });

            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>()
                .HasKey(e => new { e.CdEmpresa, e.NumeroDocumento, e.NumeroDaNFSe, e.CdProduto });

            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>()
                .HasOne(e => e.IntegracaoSppBimerInvoce)
                .WithMany(i => i.Itens)
                .HasForeignKey(e => new { e.CdEmpresa, e.NumeroDocumento, e.NumeroDaNFSe });

            modelBuilder.Entity<IntegracaoSppBimerInvoceResumo>()
                .HasKey(e => new { e.CdEmpresa, e.NumeroDocumento, e.NumeroDaNFSe });

            modelBuilder.Entity<IntegracaoSppBimerInvoceResumo>()
                .HasOne(e => e.Invoce)
                .WithMany(i => i.Resumos)
                .HasForeignKey(e => new { e.CdEmpresa, e.NumeroDocumento, e.NumeroDaNFSe });

            // Precisão dos decimais para IntegracaoSppBimerInvoceItens
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.PrecoCompraReal).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.PrecoCompraUS).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.PrecoUnitItem).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.PrecoVendaReal).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.PrecoVendaUS).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.Qtd).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoSppBimerInvoceItens>().Property(p => p.ValorTotal).HasPrecision(18, 6);

            // IntegracaoSppBimerInvoceResumo
            modelBuilder.Entity<IntegracaoSppBimerInvoceResumo>().Property(p => p.ValorPagamentoTotal).HasPrecision(18, 6);

            // IntegracaoBacenCotacaoMoeda
            modelBuilder.Entity<IntegracaoBacenCotacaoMoeda>().Property(p => p.CotacaoCompra).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoBacenCotacaoMoeda>().Property(p => p.CotacaoVenda).HasPrecision(18, 6);

            // IntegracaoBimmerInsercaoPendentes e InsertOK
            modelBuilder.Entity<IntegracaoBimmerInsercaoPendentes>().Property(p => p.Valor).HasPrecision(18, 6);
            modelBuilder.Entity<IntegracaoBimmerInsertOK>().Property(p => p.Valor).HasPrecision(18, 6);

            // IntegracaoVexpenseTitulosRelatoriosStatus
            modelBuilder.Entity<IntegracaoVexpenseTitulosRelatoriosStatus>().Property(p => p.ExpensesTotalValue).HasPrecision(18, 6);
            // Configuração direta da entidade AnyPointStoreTipoTemplate
            modelBuilder.Entity<AnyPointStoreTipoTemplate>(builder =>
            {
                builder.ToTable("AnyPointStoreTiposTemplate");

                builder.HasKey(t => t.Id);

                builder.Property(t => t.NomeCompleto)
                       .IsRequired()
                       .HasMaxLength(255);
                builder.Property(t => t.Sigla)
                       .IsRequired()
                       .HasMaxLength(20);

                builder.Property(t => t.IntegracaoId)
                       .IsRequired();

                builder.HasOne(t => t.Integracao)
                       .WithMany()
                       .HasForeignKey(t => t.IntegracaoId)
                       .OnDelete(DeleteBehavior.Restrict)
                       .HasConstraintName("FK_AnyPointStoreTiposTemplate_Integracao");
            });
        }
    }
}