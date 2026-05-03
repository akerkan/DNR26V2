using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Migrations
{
    /// <inheritdoc />
    public partial class AddEtiketModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DruckerEtikett",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EtiketButtonInAuftrag",
                table: "AppSetup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EtiketButtonInLieferschein",
                table: "AppSetup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EtiketLayoutFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LayoutName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feld = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    FontName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FontSize = table.Column<float>(type: "real", nullable: false),
                    Bold = table.Column<bool>(type: "bit", nullable: false),
                    Italic = table.Column<bool>(type: "bit", nullable: false),
                    ForeColorHex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BackColorHex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TextAlignH = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtiketLayoutFields", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EtiketLayoutFields");

            migrationBuilder.DropColumn(
                name: "DruckerEtikett",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "EtiketButtonInAuftrag",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "EtiketButtonInLieferschein",
                table: "AppSetup");
        }
    }
}
