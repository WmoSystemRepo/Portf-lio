using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DadosJson",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                newName: "PaninhaJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaninhaJson",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                newName: "DadosJson");
        }
    }
}
