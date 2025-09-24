using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "PermicoesContexto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Regras = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermicoesContexto", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointGestaoIntegracao_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");

            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointMenu_PermicoesContexto_AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropTable(
                name: "PermicoesContexto");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointMenu_AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointGestaoIntegracao_AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");

            migrationBuilder.DropColumn(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointMenu");

            migrationBuilder.DropColumn(
                name: "AnyPointStorePermicoesId",
                table: "AnyPointGestaoIntegracao");
        }
    }
}
