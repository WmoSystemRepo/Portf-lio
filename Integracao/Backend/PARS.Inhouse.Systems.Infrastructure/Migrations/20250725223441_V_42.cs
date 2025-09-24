using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adiciona StatusIntegracaoAlterData se ainda não existir
            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'StatusIntegracaoAlterData') IS NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce 
                    ADD StatusIntegracaoAlterData NVARCHAR(MAX) NULL
                END
            ");

            // Adiciona StatusIntegracaoAlterDataObs se ainda não existir
            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'StatusIntegracaoAlterDataObs') IS NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce 
                    ADD StatusIntegracaoAlterDataObs NVARCHAR(MAX) NULL
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove as colunas normalmente
            migrationBuilder.DropColumn(
                name: "StatusIntegracaoAlterData",
                table: "IntegracaoSppBimerInvoce");

            migrationBuilder.DropColumn(
                name: "StatusIntegracaoAlterDataObs",
                table: "IntegracaoSppBimerInvoce");
        }
    }
}
