using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usuario",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Versao",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usuario",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "IntegracaoAdobeHubHistoricoImportacaoExecel");
        }
    }
}
