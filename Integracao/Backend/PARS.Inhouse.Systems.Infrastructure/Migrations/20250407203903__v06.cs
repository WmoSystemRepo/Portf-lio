using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mensagem",
                table: "IntegracaoBimmerHistoricoErrosInsercoes",
                newName: "MensagemErro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MensagemErro",
                table: "IntegracaoBimmerHistoricoErrosInsercoes",
                newName: "Mensagem");
        }
    }
}
