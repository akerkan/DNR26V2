using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module4_AppSetupTurKontrolle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TurKontrolle",
                table: "AppSetup",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurKontrolle",
                table: "AppSetup");
        }
    }
}
