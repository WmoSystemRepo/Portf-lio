using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v08 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TitleId",
                table: "IntegracaoBimmerInsertOK",
                newName: "IdResponse");

            migrationBuilder.RenameColumn(
                name: "TitleId",
                table: "IntegracaoBimmerInsercaoPendentes",
                newName: "IdResponse");

            migrationBuilder.RenameColumn(
                name: "TitleId",
                table: "IntegracaoBimmerHistoricoErrosInsercoes",
                newName: "IdResponse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdResponse",
                table: "IntegracaoBimmerInsertOK",
                newName: "TitleId");

            migrationBuilder.RenameColumn(
                name: "IdResponse",
                table: "IntegracaoBimmerInsercaoPendentes",
                newName: "TitleId");

            migrationBuilder.RenameColumn(
                name: "IdResponse",
                table: "IntegracaoBimmerHistoricoErrosInsercoes",
                newName: "TitleId");
        }
    }
}
