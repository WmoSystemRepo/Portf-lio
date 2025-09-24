using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE Name = N'DataCriacao' 
                      AND Object_ID = Object_ID(N'AnyPointStoreMenu')
                )
                BEGIN
                    ALTER TABLE AnyPointStoreMenu ADD DataCriacao NVARCHAR(MAX) NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE Name = N'DataEdicao' 
                      AND Object_ID = Object_ID(N'AnyPointStoreMenu')
                )
                BEGIN
                    ALTER TABLE AnyPointStoreMenu ADD DataEdicao NVARCHAR(MAX) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE Name = N'DataCriacao' 
                      AND Object_ID = Object_ID(N'AnyPointStoreMenu')
                )
                BEGIN
                    ALTER TABLE AnyPointStoreMenu DROP COLUMN DataCriacao;
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 
                    FROM sys.columns 
                    WHERE Name = N'DataEdicao' 
                      AND Object_ID = Object_ID(N'AnyPointStoreMenu')
                )
                BEGIN
                    ALTER TABLE AnyPointStoreMenu DROP COLUMN DataEdicao;
                END
            ");
        }
    }
}
