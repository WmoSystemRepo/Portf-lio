using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCampos",
                table: "AnyPointDeparas",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointDeparas_IntegracaoMapeamentoDeCampos_IdCampos",
                table: "AnyPointDeparas");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointDeparas_IdCampos",
                table: "AnyPointDeparas");

            migrationBuilder.DropColumn(
                name: "IdCampos",
                table: "AnyPointDeparas");
        }
    }
}
