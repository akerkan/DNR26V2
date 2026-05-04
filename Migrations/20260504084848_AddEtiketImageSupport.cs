using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Migrations
{
    /// <inheritdoc />
    public partial class AddEtiketImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "EtiketLayoutFields",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageSizeMode",
                table: "EtiketLayoutFields",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "EtiketLayoutFields");

            migrationBuilder.DropColumn(
                name: "ImageSizeMode",
                table: "EtiketLayoutFields");
        }
    }
}
