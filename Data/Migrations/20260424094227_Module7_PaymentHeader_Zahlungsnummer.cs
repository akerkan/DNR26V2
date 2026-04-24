using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module7_PaymentHeader_Zahlungsnummer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Zahlungsnummer",
                table: "PaymentHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zahlungsnummer",
                table: "PaymentHeaders");
        }
    }
}
