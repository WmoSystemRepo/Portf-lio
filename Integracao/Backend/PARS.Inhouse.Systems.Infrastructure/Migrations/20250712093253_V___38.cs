using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V___38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaninhaJson",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                newName: "ExcelJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExcelJson",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                newName: "PaninhaJson");
        }
    }
}
