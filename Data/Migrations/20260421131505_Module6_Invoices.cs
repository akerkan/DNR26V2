using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module6_Invoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MengeFakturiert",
                table: "DeliveryLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rechnungsnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Rechnungsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Von = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IstSammelrechnung = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RechnungId = table.Column<int>(type: "int", nullable: false),
                    LieferscheinId = table.Column<int>(type: "int", nullable: false),
                    DeliveryLineId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    FakturierteMenge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 7.00m),
                    Gesamtpreis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Deliveries_LieferscheinId",
                        column: x => x.LieferscheinId,
                        principalTable: "Deliveries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceLines_DeliveryLines_DeliveryLineId",
                        column: x => x.DeliveryLineId,
                        principalTable: "DeliveryLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Invoices_RechnungId",
                        column: x => x.RechnungId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ArtikelId",
                table: "InvoiceLines",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_DeliveryLineId",
                table: "InvoiceLines",
                column: "DeliveryLineId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_LieferscheinId",
                table: "InvoiceLines",
                column: "LieferscheinId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_RechnungId",
                table: "InvoiceLines",
                column: "RechnungId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_KundeId",
                table: "Invoices",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Rechnungsnummer",
                table: "Invoices",
                column: "Rechnungsnummer",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropColumn(
                name: "MengeFakturiert",
                table: "DeliveryLines");
        }
    }
}
