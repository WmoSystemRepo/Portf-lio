using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V__16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoce",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CdCliente = table.Column<int>(type: "int", nullable: false),
                    NomeDoFabricante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObsNotaFiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CDOperacoes = table.Column<int>(type: "int", nullable: false),
                    DescricaoDaOperacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CDCentroDeCusto = table.Column<int>(type: "int", nullable: false),
                    DescricaoDoCentroDeCusto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoce", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe });
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoceItens",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CdProduto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomeDoProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qtd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoUnitItem = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoVendaUS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoVendaReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoCompraUS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoCompraReal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoceItens", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe, x.CdProduto });
                    table.ForeignKey(
                        name: "FK_IntegracaoSppBimerInvoceItens_IntegracaoSppBimerInvoce_CdEmpresa_NumeroDocumento_NumeroDaNFSe",
                        columns: x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe },
                        principalTable: "IntegracaoSppBimerInvoce",
                        principalColumns: new[] { "CdEmpresa", "NumeroDocumento", "NumeroDaNFSe" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoSppBimerInvoceResumo",
                columns: table => new
                {
                    CdEmpresa = table.Column<int>(type: "int", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumeroDaNFSe = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataDeVencimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorPagamentoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoSppBimerInvoceResumo", x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe });
                    table.ForeignKey(
                        name: "FK_IntegracaoSppBimerInvoceResumo_IntegracaoSppBimerInvoce_CdEmpresa_NumeroDocumento_NumeroDaNFSe",
                        columns: x => new { x.CdEmpresa, x.NumeroDocumento, x.NumeroDaNFSe },
                        principalTable: "IntegracaoSppBimerInvoce",
                        principalColumns: new[] { "CdEmpresa", "NumeroDocumento", "NumeroDaNFSe" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoceItens");

            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoceResumo");

            migrationBuilder.DropTable(
                name: "IntegracaoSppBimerInvoce");
        }
    }
}
