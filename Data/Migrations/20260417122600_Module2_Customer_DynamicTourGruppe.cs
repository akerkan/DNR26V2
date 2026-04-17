using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module2_Customer_DynamicTourGruppe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerFilter_KundenfilterId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_AusnahmeTurId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_TurId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TurId",
                table: "Customer",
                newName: "TurWertId");

            migrationBuilder.RenameColumn(
                name: "KundenfilterId",
                table: "Customer",
                newName: "KundenGruppeWertId");

            migrationBuilder.RenameColumn(
                name: "AusnahmeTurId",
                table: "Customer",
                newName: "AusnahmeTurWertId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_TurId",
                table: "Customer",
                newName: "IX_Customer_TurWertId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_KundenfilterId",
                table: "Customer",
                newName: "IX_Customer_KundenGruppeWertId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_AusnahmeTurId",
                table: "Customer",
                newName: "IX_Customer_AusnahmeTurWertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ProductAttributeValue_AusnahmeTurWertId",
                table: "Customer",
                column: "AusnahmeTurWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ProductAttributeValue_KundenGruppeWertId",
                table: "Customer",
                column: "KundenGruppeWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ProductAttributeValue_TurWertId",
                table: "Customer",
                column: "TurWertId",
                principalTable: "ProductAttributeValue",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ProductAttributeValue_AusnahmeTurWertId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ProductAttributeValue_KundenGruppeWertId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ProductAttributeValue_TurWertId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TurWertId",
                table: "Customer",
                newName: "TurId");

            migrationBuilder.RenameColumn(
                name: "KundenGruppeWertId",
                table: "Customer",
                newName: "KundenfilterId");

            migrationBuilder.RenameColumn(
                name: "AusnahmeTurWertId",
                table: "Customer",
                newName: "AusnahmeTurId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_TurWertId",
                table: "Customer",
                newName: "IX_Customer_TurId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_KundenGruppeWertId",
                table: "Customer",
                newName: "IX_Customer_KundenfilterId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_AusnahmeTurWertId",
                table: "Customer",
                newName: "IX_Customer_AusnahmeTurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerFilter_KundenfilterId",
                table: "Customer",
                column: "KundenfilterId",
                principalTable: "CustomerFilter",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Route_AusnahmeTurId",
                table: "Customer",
                column: "AusnahmeTurId",
                principalTable: "Route",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Route_TurId",
                table: "Customer",
                column: "TurId",
                principalTable: "Route",
                principalColumn: "Id");
        }
    }
}
