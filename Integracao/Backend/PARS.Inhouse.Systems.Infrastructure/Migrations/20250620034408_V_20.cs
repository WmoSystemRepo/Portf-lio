using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnyPointCadastroIntegracao",
                table: "AnyPointCadastroIntegracao");

            migrationBuilder.RenameTable(
                name: "AnyPointCadastroIntegracao",
                newName: "AnyPointGestaoIntegracao");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnyPointGestaoIntegracao",
                table: "AnyPointGestaoIntegracao",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnyPointGestaoIntegracao",
                table: "AnyPointGestaoIntegracao");

            migrationBuilder.RenameTable(
                name: "AnyPointGestaoIntegracao",
                newName: "AnyPointCadastroIntegracao");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnyPointCadastroIntegracao",
                table: "AnyPointCadastroIntegracao",
                column: "Id");
        }
    }
}
