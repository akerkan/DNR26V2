using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryTourRoutenfolgeSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Routenfolge",
                table: "Customer",
                newName: "RoutenFolgeWertId");

            migrationBuilder.AddColumn<int>(
                name: "RoutenFolgeWertId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TurWertId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
        UPDATE d
        SET 
            d.TurWertId = c.TurWertId,
            d.RoutenFolgeWertId = c.RoutenFolgeWertId
        FROM Deliveries d
        INNER JOIN Customer c ON c.Id = d.KundeId
        WHERE d.TurWertId IS NULL 
           OR d.RoutenFolgeWertId IS NULL;
    ");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_RoutenFolgeWertId",
                table: "Deliveries",
                column: "RoutenFolgeWertId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_TurWertId",
                table: "Deliveries",
                column: "TurWertId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RoutenFolgeWertId",
                table: "Customer",
                column: "RoutenFolgeWertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ProductAttributeValue_RoutenFolgeWertId",
                table: "Customer",
                column: "RoutenFolgeWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_ProductAttributeValue_RoutenFolgeWertId",
                table: "Deliveries",
                column: "RoutenFolgeWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_ProductAttributeValue_TurWertId",
                table: "Deliveries",
                column: "TurWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ProductAttributeValue_RoutenFolgeWertId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_ProductAttributeValue_RoutenFolgeWertId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_ProductAttributeValue_TurWertId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_RoutenFolgeWertId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_TurWertId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Customer_RoutenFolgeWertId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RoutenFolgeWertId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "TurWertId",
                table: "Deliveries");

            migrationBuilder.RenameColumn(
                name: "RoutenFolgeWertId",
                table: "Customer",
                newName: "Routenfolge");
        }
    }
}
