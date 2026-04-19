using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Orders_UniqueIndex_CorrectSyntax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders",
                columns: new[] { "KundeId", "LieferDatum" },
                unique: true,
                filter: "[Status] <> 2 AND [Status] <> 3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders",
                columns: new[] { "KundeId", "LieferDatum" },
                unique: true,
                filter: "[Status] NOT IN (2, 3)");
        }
    }
}
