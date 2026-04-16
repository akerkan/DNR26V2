using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module3_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Artikelnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Bezeichnung2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Einheit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "STK"),
                    VKPreis = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false, defaultValue: 0m),
                    EKPreis = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 7m),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notizen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Artikelnummer",
                table: "Product",
                column: "Artikelnummer",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
