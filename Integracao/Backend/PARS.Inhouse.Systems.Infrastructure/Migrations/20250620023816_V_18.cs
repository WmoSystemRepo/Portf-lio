using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.columns 
                    WHERE Name = N'ParentMenuId' 
                    AND Object_ID = Object_ID(N'AnyPointMenu')
                )
                ALTER TABLE AnyPointMenu ADD ParentMenuId int NULL;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "DataCriacao",
                table: "AnyPointCadastroIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "DataEdicao",
                table: "AnyPointCadastroIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AnyPointCadastroIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEdicao",
                table: "AnyPointCadastroIntegracao");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AnyPointCadastroIntegracao");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "AnyPointCadastroIntegracao",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM sys.columns 
                    WHERE Name = N'ParentMenuId' 
                    AND Object_ID = Object_ID(N'AnyPointMenu')
                )
                ALTER TABLE AnyPointMenu DROP COLUMN ParentMenuId;
            ");
        }
    }
}
