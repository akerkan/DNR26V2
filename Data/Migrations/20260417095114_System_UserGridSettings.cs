using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class System_UserGridSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGridSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Benutzername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GridKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Einstellungen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGridSetting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGridSetting_Benutzername_GridKey",
                table: "UserGridSetting",
                columns: new[] { "Benutzername", "GridKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGridSetting");
        }
    }
}
