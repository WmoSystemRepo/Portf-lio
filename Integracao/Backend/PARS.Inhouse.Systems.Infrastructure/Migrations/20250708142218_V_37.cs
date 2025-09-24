using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuPermicoes");

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreMenuUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuUsuario_AnyPointStoreMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointStoreMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuUsuario_AnyPointStorePermicoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "AnyPointStorePermicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuUsuario_MenuId",
                table: "AnyPointStoreMenuUsuario",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuUsuario_PermissaoId",
                table: "AnyPointStoreMenuUsuario",
                column: "PermissaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointStoreMenuUsuario");

            migrationBuilder.CreateTable(
                name: "AnyPointStoreMenuPermicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_AnyPointStoreMenuPermicoes_AnyPointStorePermicoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "AnyPointStorePermicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuPermicoes_MenuId",
                table: "AnyPointStoreMenuPermicoes",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuPermicoes_PermissaoId",
                table: "AnyPointStoreMenuPermicoes",
                column: "PermissaoId");
        }
    }
}
