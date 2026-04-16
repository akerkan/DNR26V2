using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class _001_InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Firmenname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Firmenadresse = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FirmenPLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FirmenOrt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirmenLand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Deutschland"),
                    FirmenTelefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirmenEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FirmenSteuernummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirmenUStIdNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StandardMwstProzent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 7.00m),
                    StandardStandortCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DruckerWeissesPapier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DruckerMitLogo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RechnungPraefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "RE"),
                    LieferscheinPraefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "LS"),
                    GutschriftPraefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "GS"),
                    ZahlungPraefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "ZA"),
                    SeitenGroesse = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSetup", x => x.Id);
                    table.CheckConstraint("CK_AppSetup_Singleton", "[Id] = 1");
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tabellenname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatensatzId = table.Column<int>(type: "int", nullable: false),
                    Belegnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aktion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlterWert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeuerWert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grund = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Benutzer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Zeitstempel = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IPAdresse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFilter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kundenfilter = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFilter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Driver",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fahrercode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Vorname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nachname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Standortcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Land = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Deutschland"),
                    IstStandard = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Seriencode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Beschreibung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Praefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LetzteVerwendeteNr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LetztesVerwendetesDatum = table.Column<DateTime>(type: "date", nullable: true),
                    Nummernformat = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "000"),
                    Trennzeichen = table.Column<string>(type: "nchar(1)", nullable: false, defaultValue: "-"),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Routencode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IstWochenendtour = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kundennummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Kundenname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Inhaber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Adresse2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Land = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Deutschland"),
                    Telefonnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Handynummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AbweichendeLieferadresse = table.Column<bool>(type: "bit", nullable: false),
                    ALName2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALInhaber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALAdresse = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALAdresse2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALPLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ALOrt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ALLand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    Routenfolge = table.Column<int>(type: "int", nullable: false),
                    KundenfilterId = table.Column<int>(type: "int", nullable: true),
                    Wochenendtour = table.Column<bool>(type: "bit", nullable: false),
                    Limit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    PreisAusblenden = table.Column<bool>(type: "bit", nullable: false),
                    Geraete1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Geraete2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Geraete3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Geraete4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Geraete5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Offen = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notizen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerFilter_KundenfilterId",
                        column: x => x.KundenfilterId,
                        principalTable: "CustomerFilter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Customer_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Benutzer",
                table: "AuditLog",
                column: "Benutzer");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Tabellenname",
                table: "AuditLog",
                column: "Tabellenname");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Zeitstempel",
                table: "AuditLog",
                column: "Zeitstempel");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_KundenfilterId",
                table: "Customer",
                column: "KundenfilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Kundennummer",
                table: "Customer",
                column: "Kundennummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RouteId",
                table: "Customer",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFilter_Kundenfilter",
                table: "CustomerFilter",
                column: "Kundenfilter",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Driver_Fahrercode",
                table: "Driver",
                column: "Fahrercode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_Standortcode",
                table: "Location",
                column: "Standortcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoSeries_Seriencode",
                table: "NoSeries",
                column: "Seriencode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Route_Routencode",
                table: "Route",
                column: "Routencode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSetup");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "NoSeries");

            migrationBuilder.DropTable(
                name: "CustomerFilter");

            migrationBuilder.DropTable(
                name: "Route");
        }
    }
}
