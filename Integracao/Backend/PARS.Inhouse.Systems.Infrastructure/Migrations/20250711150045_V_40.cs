// File: Migrations/V_40.cs

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    public partial class V_40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Checa se a coluna 'PermissaoId' existe antes de remover FK, índice e a coluna
            var checkColumnExistsSql = @"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'AnyPointStoreMenuUsuario' 
                    AND COLUMN_NAME = 'PermissaoId'
                )
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM sys.foreign_keys 
                        WHERE name = 'FK_AnyPointStoreMenuUsuario_AnyPointStorePermicoes_PermissaoId')
                        ALTER TABLE AnyPointStoreMenuUsuario DROP CONSTRAINT FK_AnyPointStoreMenuUsuario_AnyPointStorePermicoes_PermissaoId;

                    IF EXISTS (
                        SELECT 1 FROM sys.indexes 
                        WHERE name = 'IX_AnyPointStoreMenuUsuario_PermissaoId')
                        DROP INDEX IX_AnyPointStoreMenuUsuario_PermissaoId ON AnyPointStoreMenuUsuario;

                    ALTER TABLE AnyPointStoreMenuUsuario DROP COLUMN PermissaoId;
                END";

            migrationBuilder.Sql(checkColumnExistsSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissaoId",
                table: "AnyPointStoreMenuUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuUsuario_PermissaoId",
                table: "AnyPointStoreMenuUsuario",
                column: "PermissaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointStoreMenuUsuario_AnyPointStorePermicoes_PermissaoId",
                table: "AnyPointStoreMenuUsuario",
                column: "PermissaoId",
                principalTable: "AnyPointStorePermicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
