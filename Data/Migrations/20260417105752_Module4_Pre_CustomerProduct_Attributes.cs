using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module4_Pre_CustomerProduct_Attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductAttribute_Bezeichnung",
                table: "ProductAttribute");

            migrationBuilder.AlterColumn<string>(
                name: "Bezeichnung",
                table: "ProductAttribute",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "ProductAttribute",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "CustomerProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Gewicht = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductAttributeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProductId = table.Column<int>(type: "int", nullable: false),
                    AttributId = table.Column<int>(type: "int", nullable: false),
                    AttributWertId = table.Column<int>(type: "int", nullable: true),
                    FreierText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductAttributeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_CustomerProduct_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_ProductAttributeValue_AttributWertId",
                        column: x => x.AttributWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_ProductAttribute_AttributId",
                        column: x => x.AttributId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProduct_ArtikelId",
                table: "CustomerProduct",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProduct_KundeId_ArtikelId",
                table: "CustomerProduct",
                columns: new[] { "KundeId", "ArtikelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_AttributId",
                table: "CustomerProductAttributeMapping",
                column: "AttributId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_AttributWertId",
                table: "CustomerProductAttributeMapping",
                column: "AttributWertId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_CustomerProductId",
                table: "CustomerProductAttributeMapping",
                column: "CustomerProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProductAttributeMapping");

            migrationBuilder.DropTable(
                name: "CustomerProduct");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ProductAttribute");

            migrationBuilder.AlterColumn<string>(
                name: "Bezeichnung",
                table: "ProductAttribute",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_Bezeichnung",
                table: "ProductAttribute",
                column: "Bezeichnung",
                unique: true);
        }
    }
}
