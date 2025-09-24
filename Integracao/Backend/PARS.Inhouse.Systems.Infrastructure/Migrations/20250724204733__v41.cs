using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARS.Inhouse.Systems.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _v41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubMenuReferencaiPrincipal",
                table: "AnyPointStoreMenu",
                newName: "SubMenuReferenciaPrincipal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubMenuReferenciaPrincipal",
                table: "AnyPointStoreMenu",
                newName: "SubMenuReferencaiPrincipal");
        }
    }
}
