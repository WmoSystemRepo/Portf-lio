using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                table: "AnyPointPermissaoUsuario");

            migrationBuilder.DropTable(
                name: "DeParaEmpresas");

            migrationBuilder.DropTable(
                name: "DeParaVexpensesBimmer");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "TituloAprovadoDespesa");

            migrationBuilder.DropTable(
                name: "TitulosPagos");

            migrationBuilder.DropTable(
                name: "TitulosAprovados");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AnyPointUserEndpointPermission",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PermissaoId",
                table: "AnyPointPermissaoUsuario",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "AnyPointPermissaoUsuario",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Deparas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoExecucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Integracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorDestino = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deparas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoBimmerInsertOK",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    IdentificadorBimmer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoBimmerInsertOK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegracaoVexpenseTitulosRelatoriosStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResponse = table.Column<int>(type: "int", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: true),
                    Device_id = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approval_stage_id = table.Column<int>(type: "int", nullable: true),
                    Approval_user_id = table.Column<int>(type: "int", nullable: true),
                    Approval_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payment_method_id = table.Column<int>(type: "int", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paying_company_id = table.Column<int>(type: "int", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pdf_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Excel_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpensesTotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpensesData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracaoVexpenseTitulosRelatoriosStatus", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                table: "AnyPointPermissaoUsuario",
                column: "PermissaoId",
                principalTable: "AnyPointPermissao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                table: "AnyPointPermissaoUsuario");

            migrationBuilder.DropTable(
                name: "Deparas");

            migrationBuilder.DropTable(
                name: "IntegracaoBimmerInsertOK");

            migrationBuilder.DropTable(
                name: "IntegracaoVexpenseTitulosRelatoriosStatus");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AnyPointUserEndpointPermission",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PermissaoId",
                table: "AnyPointPermissaoUsuario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "AnyPointPermissaoUsuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DeParaEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeParaEmpresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeParaVexpensesBimmer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoItemDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoItemOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoItemDestino = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescricaoItemOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjetoOrigem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeParaVexpensesBimmer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitulosAprovados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approval_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Approval_stage_id = table.Column<int>(type: "int", nullable: true),
                    Approval_user_id = table.Column<int>(type: "int", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Device_id = table.Column<int>(type: "int", nullable: true),
                    Excel_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdResponse = table.Column<int>(type: "int", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: false),
                    Paying_company_id = table.Column<int>(type: "int", nullable: true),
                    Payment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payment_method_id = table.Column<int>(type: "int", nullable: true),
                    Pdf_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitulosAprovados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitulosPagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentificadorBimmer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitulosPagos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TituloAprovadoDespesa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTituloAprovado = table.Column<int>(type: "int", nullable: true),
                    ConvertedCurrencyIso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvertedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpenseId = table.Column<int>(type: "int", nullable: true),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    IdResponse = table.Column<int>(type: "int", nullable: true),
                    IntegrationId = table.Column<int>(type: "int", nullable: true),
                    Mileage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MileageValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    On = table.Column<bool>(type: "bit", nullable: true),
                    OriginalCurrencyIso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayingCompanyId = table.Column<int>(type: "int", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    ReiceptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reimbursable = table.Column<bool>(type: "bit", nullable: true),
                    Rejected = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Validate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TituloAprovadoDespesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TituloAprovadoDespesa_TitulosAprovados_IdTituloAprovado",
                        column: x => x.IdTituloAprovado,
                        principalTable: "TitulosAprovados",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TituloAprovadoDespesa_IdTituloAprovado",
                table: "TituloAprovadoDespesa",
                column: "IdTituloAprovado");

            migrationBuilder.AddForeignKey(
                name: "FK_AnyPointPermissaoUsuario_AnyPointPermissao_PermissaoId",
                table: "AnyPointPermissaoUsuario",
                column: "PermissaoId",
                principalTable: "AnyPointPermissao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
