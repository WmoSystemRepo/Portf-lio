using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ... (mantido conforme versão anterior) ...
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AnyPointStoreGestaoIntegracao");
            migrationBuilder.DropTable(name: "AnyPointStoreMenuIntegracao");
            migrationBuilder.DropTable(name: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnyPointStorePermicoes",
                table: "AnyPointStorePermicoes");

            migrationBuilder.DropColumn(name: "DataCriacao", table: "AnyPointStoreMenu");
            migrationBuilder.DropColumn(name: "DataEdicao", table: "AnyPointStoreMenu");
            migrationBuilder.DropColumn(name: "RegrasIds", table: "AnyPointStoreMenu");

            migrationBuilder.RenameTable(
                name: "AnyPointStorePermicoes",
                newName: "PermicoesContexto");

            migrationBuilder.AddColumn<int>(
                name: "PermissaoId",
                table: "AnyPointPermissaoUsuario",
                type: "int",
                nullable: true);

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
                name: "AnyPointMenu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrdemMenu = table.Column<int>(type: "int", nullable: true),
                    Rota = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointMenu_AnyPointMenu_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnyPointPermissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointPermissao", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "AnyPointMenuAnyPointPermissao",
                columns: table => new
                {
                    MenusId = table.Column<int>(type: "int", nullable: false),
                    PermissoesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenuAnyPointPermissao", x => new { x.MenusId, x.PermissoesId });
                    table.ForeignKey(
                        name: "FK_AnyPointMenuAnyPointPermissao_AnyPointMenu_MenusId",
                        column: x => x.MenusId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuAnyPointPermissao_AnyPointPermissao_PermissoesId",
                        column: x => x.PermissoesId,
                        principalTable: "AnyPointPermissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnyPointMenuPermissao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointMenuPermissao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuPermissao_AnyPointMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "AnyPointMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnyPointMenuPermissao_AnyPointPermissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "AnyPointPermissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointUserEndpointPermission_MenuId",
                table: "AnyPointUserEndpointPermission",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointPermissaoUsuario_PermissaoId",
                table: "AnyPointPermissaoUsuario",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenu_ParentMenuId",
                table: "AnyPointMenu",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuAnyPointPermissao_PermissoesId",
                table: "AnyPointMenuAnyPointPermissao",
                column: "PermissoesId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuPermissao_MenuId",
                table: "AnyPointMenuPermissao",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointMenuPermissao_PermissaoId",
                table: "AnyPointMenuPermissao",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_GestaoIntegracoesDto_AnyPointStoreMenuId",
                table: "GestaoIntegracoesDto",
                column: "AnyPointStoreMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoDto_AnyPointStoreMenuId",
                table: "PermissaoDto",
                column: "AnyPointStoreMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                table: "AnyPointPermissaoUsuario",
                column: "PermissaoId",
                principalTable: "AnyPointPermissao",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointUserEndpointPermission_AnyPointMenu_MenuId",
                table: "AnyPointUserEndpointPermission",
                column: "MenuId",
                principalTable: "AnyPointMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
