using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module3_Product_Felder_Printfarbe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VKPreis",
                table: "Product",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldPrecision: 10,
                oldScale: 4,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Notizen",
                table: "Product",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EKPreis",
                table: "Product",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldPrecision: 10,
                oldScale: 4,
                oldDefaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Feld1",
                table: "Product",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feld2",
                table: "Product",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feld3",
                table: "Product",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feld4",
                table: "Product",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Printfarbe",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feld1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Feld2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Feld3",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Feld4",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Printfarbe",
                table: "Product");

            migrationBuilder.AlterColumn<decimal>(
                name: "VKPreis",
                table: "Product",
                type: "decimal(10,4)",
                precision: 10,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Notizen",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EKPreis",
                table: "Product",
                type: "decimal(10,4)",
                precision: 10,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldDefaultValue: 0m);
        }
    }
}
