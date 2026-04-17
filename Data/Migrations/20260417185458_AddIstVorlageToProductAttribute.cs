using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIstVorlageToProductAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IstVorlage",
                table: "ProductAttributeValue");

            migrationBuilder.AlterColumn<int>(
                name: "EntityType",
                table: "ProductAttribute",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Bezeichnung",
                table: "ProductAttribute",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "IstVorlage",
                table: "ProductAttribute",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_Bezeichnung",
                table: "ProductAttribute",
                column: "Bezeichnung",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductAttribute_Bezeichnung",
                table: "ProductAttribute");

            migrationBuilder.DropColumn(
                name: "IstVorlage",
                table: "ProductAttribute");

            migrationBuilder.AddColumn<bool>(
                name: "IstVorlage",
                table: "ProductAttributeValue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "EntityType",
                table: "ProductAttribute",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Bezeichnung",
                table: "ProductAttribute",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
