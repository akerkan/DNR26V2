using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module2_Remove_CustomerFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerFilter_CustomerFilterId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerFilter");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerFilterId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerFilterId",
                table: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerFilterId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerFilter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kundenfilter = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFilter", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerFilterId",
                table: "Customer",
                column: "CustomerFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFilter_Kundenfilter",
                table: "CustomerFilter",
                column: "Kundenfilter",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerFilter_CustomerFilterId",
                table: "Customer",
                column: "CustomerFilterId",
                principalTable: "CustomerFilter",
                principalColumn: "Id");
        }
    }
}
