using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_44 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnyPointStoreTiposTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeCompleto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Sigla = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IntegracaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnyPointStoreTiposTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnyPointStoreTiposTemplate_Integracao",
                        column: x => x.IntegracaoId,
                        principalTable: "AnyPointStoreGestaoIntegracao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreTiposTemplate_IntegracaoId",
                table: "AnyPointStoreTiposTemplate",
                column: "IntegracaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnyPointStoreTiposTemplate");
        }
    }
}
