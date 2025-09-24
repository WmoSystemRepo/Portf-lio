using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropColumn(
                name: "ProjetoDestino",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropColumn(
                name: "ProjetoOrigem",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropColumn(
                name: "RegrasIds",
                table: "AnyPointStoreMenu");

            migrationBuilder.AlterColumn<string>(
                name: "DataEdicao",
                table: "AnyPointStoreMenuPermicoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DataCriacao",
                table: "AnyPointStoreMenuPermicoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "AnyPointStoreMenuPermicoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PermissaoId",
                table: "AnyPointStoreMenuPermicoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "DataEdicao",
                table: "AnyPointStoreMenuIntegracao",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DataCriacao",
                table: "AnyPointStoreMenuIntegracao",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "AnyPointStoreMenuIntegracao",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IntegracaoId",
                table: "AnyPointStoreMenuIntegracao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuPermicoes_PermissaoId",
                table: "AnyPointStoreMenuPermicoes",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnyPointStoreMenuIntegracao_IntegracaoId",
                table: "AnyPointStoreMenuIntegracao",
                column: "IntegracaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointStoreMenuIntegracao_AnyPointStoreGestaoIntegracao_IntegracaoId",
                table: "AnyPointStoreMenuIntegracao",
                column: "IntegracaoId",
                principalTable: "AnyPointStoreGestaoIntegracao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointStoreMenuPermicoes_AnyPointStorePermicoes_PermissaoId",
                table: "AnyPointStoreMenuPermicoes",
                column: "PermissaoId",
                principalTable: "AnyPointStorePermicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointStoreMenuIntegracao_AnyPointStoreGestaoIntegracao_IntegracaoId",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointStoreMenuPermicoes_AnyPointStorePermicoes_PermissaoId",
                table: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointStoreMenuPermicoes_PermissaoId",
                table: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropIndex(
                name: "IX_AnyPointStoreMenuIntegracao_IntegracaoId",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropColumn(
                name: "PermissaoId",
                table: "AnyPointStoreMenuPermicoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.DropColumn(
                name: "IntegracaoId",
                table: "AnyPointStoreMenuIntegracao");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEdicao",
                table: "AnyPointStoreMenuPermicoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "AnyPointStoreMenuPermicoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AnyPointStoreMenuPermicoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEdicao",
                table: "AnyPointStoreMenuIntegracao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "AnyPointStoreMenuIntegracao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AnyPointStoreMenuIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoDestino",
                table: "AnyPointStoreMenuIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoOrigem",
                table: "AnyPointStoreMenuIntegracao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegrasIds",
                table: "AnyPointStoreMenu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
