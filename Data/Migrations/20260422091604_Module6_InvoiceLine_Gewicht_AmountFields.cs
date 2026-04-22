using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module6_InvoiceLine_Gewicht_AmountFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gesamtpreis",
                table: "InvoiceLines");

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtbrutto",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtmwst",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtnetto",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInclVat",
                table: "OrderLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "OrderLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountProzent",
                table: "OrderLines",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LineAmount",
                table: "OrderLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "OrderLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtbrutto",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtmwst",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtnetto",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInclVat",
                table: "InvoiceLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "InvoiceLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountProzent",
                table: "InvoiceLines",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "InvoiceLines",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LineAmount",
                table: "InvoiceLines",
                type: "decimal(18,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "InvoiceLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInclVat",
                table: "DeliveryLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "DeliveryLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountProzent",
                table: "DeliveryLines",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAmount",
                table: "DeliveryLines",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LineAmount",
                table: "DeliveryLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MwstProzent",
                table: "DeliveryLines",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 7.00m);

            migrationBuilder.AddColumn<decimal>(
                name: "VatAmount",
                table: "DeliveryLines",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtbrutto",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtmwst",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtnetto",
                table: "Deliveries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PreisFormel",
                table: "AppSetup",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gesamtbrutto",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Gesamtmwst",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Gesamtnetto",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AmountInclVat",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "DiscountProzent",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "LineAmount",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "Gesamtbrutto",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Gesamtmwst",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Gesamtnetto",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "AmountInclVat",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "DiscountProzent",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "LineAmount",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "InvoiceLines");

            migrationBuilder.DropColumn(
                name: "AmountInclVat",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "DiscountProzent",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "GrossAmount",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "LineAmount",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "MwstProzent",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "VatAmount",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "Gesamtbrutto",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Gesamtmwst",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Gesamtnetto",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PreisFormel",
                table: "AppSetup");

            migrationBuilder.AddColumn<decimal>(
                name: "Gesamtpreis",
                table: "InvoiceLines",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
