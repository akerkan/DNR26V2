using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Migrations
{
    /// <inheritdoc />
    public partial class AddEtiketPaperConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EtiketPaperConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LayoutName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WidthCm = table.Column<float>(type: "real", nullable: false),
                    HeightCm = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtiketPaperConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EtiketPaperConfigs");
        }
    }
}
