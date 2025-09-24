using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointGestaoIntegracao_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");

            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointMenu_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointMenu_AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointGestaoIntegracao_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");

            migrationBuilder.DropColumn(
                name: "Regras",
                table: "PermicoesContexto");

            migrationBuilder.DropColumn(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropColumn(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Regras",
                table: "PermicoesContexto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointMenu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenu_AnyPointStorePermicoesId",
                table: "AnyPointMenu",
                column: "AnyPointStorePermicoesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointGestaoIntegracao_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao",
                column: "AnyPointStorePermicoesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointGestaoIntegracao_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao",
                column: "AnyPointStorePermicoesId",
                principalTable: "PermicoesContexto",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointMenu_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointMenu",
                column: "AnyPointStorePermicoesId",
                principalTable: "PermicoesContexto",
                principalColumn: "Id");
        }
    }
}
