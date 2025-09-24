using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deparas");

            migrationBuilder.CreateTable(
                name: "AnyPointDeparas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoExecucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Integracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorDestino = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointDeparas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointDeparas");

            migrationBuilder.CreateTable(
                name: "Deparas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Integracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoExecucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deparas", x => x.Id);
                });
        }
    }
}
