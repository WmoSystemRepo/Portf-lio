using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_47 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'Estoque') IS NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce 
                    ADD Estoque NVARCHAR(5) NOT NULL DEFAULT('');
                END
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'Fabricante') IS NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce 
                    ADD Fabricante INT NOT NULL DEFAULT(0);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'Estoque') IS NOT NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce DROP COLUMN Estoque;
                END
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('IntegracaoSppBimerInvoce', 'Fabricante') IS NOT NULL
                BEGIN
                    ALTER TABLE IntegracaoSppBimerInvoce DROP COLUMN Fabricante;
                END
            ");
        }
    }
}
