using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module5_Komplet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "BIC",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderFreigegeben",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderGebucht",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderLabelFreigegeben",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderLabelGebucht",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderLabelOffen",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderLabelStorniert",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderOffen",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ColorOrderStorniert",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kontoinhaber",
                table: "AppSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SkontoProzent",
                table: "AppSetup",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SkontoTage",
                table: "AppSetup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZahlungszielTage",
                table: "AppSetup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders",
                columns: new[] { "KundeId", "LieferDatum" },
                unique: true,
                filter: "[Status] <> 3 AND [Status] <> 4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BIC",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderFreigegeben",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderGebucht",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderLabelFreigegeben",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderLabelGebucht",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderLabelOffen",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderLabelStorniert",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderOffen",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ColorOrderStorniert",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "Kontoinhaber",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "SkontoProzent",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "SkontoTage",
                table: "AppSetup");

            migrationBuilder.DropColumn(
                name: "ZahlungszielTage",
                table: "AppSetup");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders",
                columns: new[] { "KundeId", "LieferDatum" },
                unique: true,
                filter: "[Status] <> 2 AND [Status] <> 3 AND [Status] <> 4");
        }
    }
}
