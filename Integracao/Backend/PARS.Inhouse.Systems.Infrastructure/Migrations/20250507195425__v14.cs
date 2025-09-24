using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegracaoBacenCotacaoMoeda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoMoeda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHoraCotacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CotacaoCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CotacaoVenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoBoletim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataHoraIntegracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBacenCotacaoMoeda", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegracaoBacenCotacaoMoeda");
        }
    }
}
