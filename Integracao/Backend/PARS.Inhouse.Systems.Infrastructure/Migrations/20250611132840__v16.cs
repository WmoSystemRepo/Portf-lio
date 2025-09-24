using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplatesPlanilhaCampos");

            migrationBuilder.DropTable(
                name: "TemplatesPlanilha");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "AnyPointCadastroIntegracao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "AnyPointCadastroIntegracao");

            migrationBuilder.CreateTable(
                name: "TemplatesPlanilha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesPlanilha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplatesPlanilhaCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplatesPlanilhaId = table.Column<int>(type: "int", nullable: false),
                    CampoFixo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColunaBanco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LetraColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomePersonalizadoColunaPlanilha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreencherCampoEmBranco = table.Column<bool>(type: "bit", nullable: false),
                    PreencherCampoFixo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesPlanilhaCampos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplatesPlanilhaCampos_TemplatesPlanilha_TemplatesPlanilhaId",
                        column: x => x.TemplatesPlanilhaId,
                        principalTable: "TemplatesPlanilha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplatesPlanilhaCampos_TemplatesPlanilhaId",
                table: "TemplatesPlanilhaCampos",
                column: "TemplatesPlanilhaId");
        }
    }
}
