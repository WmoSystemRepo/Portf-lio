using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomeMapeamento",
                table: "AnyPointDeparas",
                newName: "NomeMapeamentoOrigem");

            migrationBuilder.AddColumn<string>(
                name: "NomeMapeamentoDestino",
                table: "AnyPointDeparas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IntegracaoMapeamentoDeCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampoOrigemSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampoDestinoSistema = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoMapeamentoDeCampos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegracaoMapeamentoDeCampos");

            migrationBuilder.DropColumn(
                name: "NomeMapeamentoDestino",
                table: "AnyPointDeparas");

            migrationBuilder.RenameColumn(
                name: "NomeMapeamentoOrigem",
                table: "AnyPointDeparas",
                newName: "NomeMapeamento");
        }
    }
}
