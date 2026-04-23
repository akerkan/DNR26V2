using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module6_InvoiceDocumentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BelegArt",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OriginalRechnungId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OriginalRechnungId",
                table: "Invoices",
                column: "OriginalRechnungId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Invoices_OriginalRechnungId",
                table: "Invoices",
                column: "OriginalRechnungId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Invoices_OriginalRechnungId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OriginalRechnungId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "BelegArt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "OriginalRechnungId",
                table: "Invoices");
        }
    }
}
