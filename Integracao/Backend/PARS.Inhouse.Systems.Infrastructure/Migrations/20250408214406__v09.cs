using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v09 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataPagamento",
                table: "IntegracaoBimmerInsertOK",
                newName: "DataCadastro");

            migrationBuilder.RenameColumn(
                name: "DataRegistro",
                table: "IntegracaoBimmerInsercaoPendentes",
                newName: "DataCadastro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCadastro",
                table: "IntegracaoBimmerInsertOK",
                newName: "DataPagamento");

            migrationBuilder.RenameColumn(
                name: "DataCadastro",
                table: "IntegracaoBimmerInsercaoPendentes",
                newName: "DataRegistro");
        }
    }
}
