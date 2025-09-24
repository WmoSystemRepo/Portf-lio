using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyPointStoreUsuarioIntegracao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    IntegracaoId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEdicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreUsuarioIntegracao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreUsuarioIntegracao_AnyPointStoreGestaoIntegracao_IntegracaoId",
                        column: x => x.IntegracaoId,
                        principalTable: "AnyPointStoreGestaoIntegracao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreUsuarioIntegracao_IntegracaoId",
                table: "AnyPointStoreUsuarioIntegracao",
                column: "IntegracaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointStoreUsuarioIntegracao");
        }
    }
}
