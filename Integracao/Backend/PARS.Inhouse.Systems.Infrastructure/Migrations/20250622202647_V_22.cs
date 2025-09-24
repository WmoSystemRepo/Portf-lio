using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrdenacaoMenu = table.Column<int>(type: "int", nullable: true),
                    EhMenuPrincipal = table.Column<bool>(type: "bit", nullable: true),
                    SubMenuReferencaiPrincipal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GestaoIntegracoesDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnyPointStoreMenuId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GestaoIntegracoesDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GestaoIntegracoesDto_AnyPointStoreMenu_AnyPointStoreMenuId",
                        column: x => x.AnyPointStoreMenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissaoDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnyPointStoreMenuId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissaoDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissaoDto_AnyPointStoreMenu_AnyPointStoreMenuId",
                        column: x => x.AnyPointStoreMenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GestaoIntegracoesDto_AnyPointStoreMenuId",
                table: "GestaoIntegracoesDto",
                column: "AnyPointStoreMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoDto_AnyPointStoreMenuId",
                table: "PermissaoDto",
                column: "AnyPointStoreMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GestaoIntegracoesDto");

            migrationBuilder.DropTable(
                name: "PermissaoDto");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenu");
        }
    }
}
