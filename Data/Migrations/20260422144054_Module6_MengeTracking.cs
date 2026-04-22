using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module6_MengeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MengeFakturiert",
                table: "OrderLines",
                type: "decimal(10,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MengeGeliefert",
                table: "OrderLines",
                type: "decimal(10,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AuftragZeileId",
                table: "DeliveryLines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_AuftragZeileId",
                table: "DeliveryLines",
                column: "AuftragZeileId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryLines_OrderLines_AuftragZeileId",
                table: "DeliveryLines",
                column: "AuftragZeileId",
                principalTable: "OrderLines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryLines_OrderLines_AuftragZeileId",
                table: "DeliveryLines");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryLines_AuftragZeileId",
                table: "DeliveryLines");

            migrationBuilder.DropColumn(
                name: "MengeFakturiert",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "MengeGeliefert",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "AuftragZeileId",
                table: "DeliveryLines");
        }
    }
}
