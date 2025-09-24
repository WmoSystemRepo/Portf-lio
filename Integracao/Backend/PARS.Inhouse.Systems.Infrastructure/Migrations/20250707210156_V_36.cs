using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointDeparas_IntegracaoMapeamentoDeCampos_IdCampos",
                table: "AnyPointDeparas");

            migrationBuilder.DropTable(
                name: "IntegracaoMapeamentoDeCampos");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointDeparas_IdCampos",
                table: "AnyPointDeparas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegracaoMapeamentoDeCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampoDestinoSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampoOrigemSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoIntegracao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoMapeamentoDeCampos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointDeparas_IdCampos",
                table: "AnyPointDeparas",
                column: "IdCampos");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointDeparas_IntegracaoMapeamentoDeCampos_IdCampos",
                table: "AnyPointDeparas",
                column: "IdCampos",
                principalTable: "IntegracaoMapeamentoDeCampos",
                principalColumn: "Id");
        }
    }
}
