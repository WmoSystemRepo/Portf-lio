using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V_17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "AnyPointMenu");

            migrationBuilder.AddColumn<int>(
                name: "OrdemMenu",
                table: "AnyPointMenu",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrdemMenu",
                table: "AnyPointMenu");

            migrationBuilder.AddColumn<double>(
                name: "Position",
                table: "AnyPointMenu",
                type: "float",
                nullable: true);
        }
    }
}
