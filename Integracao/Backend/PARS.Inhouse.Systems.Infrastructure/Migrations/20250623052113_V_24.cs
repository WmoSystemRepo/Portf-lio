using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointGestaoIntegracao");

            migrationBuilder.DropTable(
                name: "GestaoIntegracoesDto");

            migrationBuilder.DropTable(
                name: "PermissaoDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermicoesContexto",
                table: "PermicoesContexto");

            migrationBuilder.RenameTable(
                name: "PermicoesContexto",
                newName: "AnyPointStorePermicoes");

            migrationBuilder.AddColumn<string>(
                name: "RegrasIds",
                table: "AnyPointStoreMenu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnyPointStorePermicoes",
                table: "AnyPointStorePermicoes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AnyPointStoreGestaoIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreGestaoIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenuIntegracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuIntegracao_AnyPointStoreMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuPermicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEdicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenuPermicoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuPermicoes_AnyPointStoreMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuIntegracao_MenuId",
                table: "AnyPointStoreMenuIntegracao",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuPermicoes_MenuId",
                table: "AnyPointStoreMenuPermicoes",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointStoreGestaoIntegracao");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnyPointStorePermicoes",
                table: "AnyPointStorePermicoes");

            migrationBuilder.DropColumn(
                name: "RegrasIds",
                table: "AnyPointStoreMenu");

            migrationBuilder.RenameTable(
                name: "AnyPointStorePermicoes",
                newName: "PermicoesContexto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermicoesContexto",
                table: "PermicoesContexto",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AnyPointGestaoIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointGestaoIntegracao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GestaoIntegracoesDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnyPointStoreMenuId = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    AnyPointStoreMenuId = table.Column<int>(type: "int", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
    }
}
