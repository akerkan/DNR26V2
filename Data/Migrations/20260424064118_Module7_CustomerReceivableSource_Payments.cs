using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module7_CustomerReceivableSource_Payments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceivableSource",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "PaymentHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Buchungsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentHeaderId = table.Column<int>(type: "int", nullable: false),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErstelltVon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentLines_PaymentHeaders_PaymentHeaderId",
                        column: x => x.PaymentHeaderId,
                        principalTable: "PaymentHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLines_PaymentHeaderId",
                table: "PaymentLines",
                column: "PaymentHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentLines");

            migrationBuilder.DropTable(
                name: "PaymentHeaders");

            migrationBuilder.DropColumn(
                name: "ReceivableSource",
                table: "Customer");
        }
    }
}
