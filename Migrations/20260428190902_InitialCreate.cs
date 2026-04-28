using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    AuftraegeArchivieren = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TurKontrolle = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ColorOrderOffen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorOrderFreigegeben = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorOrderGebucht = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorOrderStorniert = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorOrderLabelOffen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorOrderLabelFreigegeben = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorOrderLabelGebucht = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorOrderLabelStorniert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kontoinhaber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZahlungszielTage = table.Column<int>(type: "int", nullable: false),
                    SkontoProzent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SkontoTage = table.Column<int>(type: "int", nullable: false),
                    PreisFormel = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LogoVerwenden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LogoPfad = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BriefpapierVerwenden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
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
                name: "PaymentHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zahlungsnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Buchungsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Artikelnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Bezeichnung2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Einheit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "STK"),
                    VKPreis = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    EKPreis = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 7m),
                    PreisFormel = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Feld1 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Feld2 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Feld3 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Feld4 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Printfarbe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notizen = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Feldtyp = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    MaxLaenge = table.Column<int>(type: "int", nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IstVorlage = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "ProductAttributeValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributId = table.Column<int>(type: "int", nullable: false),
                    Bezeichnung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sortierung = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValue_ProductAttribute_AttributId",
                        column: x => x.AttributId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Adresse = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Adresse2 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Land = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Deutschland"),
                    Telefonnummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Handynummer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AbweichendeLieferadresse = table.Column<bool>(type: "bit", nullable: false),
                    ALName2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALInhaber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ALAdresse = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ALAdresse2 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ALPLZ = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ALOrt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ALLand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Routenfolge = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TurWertId = table.Column<int>(type: "int", nullable: true),
                    AusnahmeTurWertId = table.Column<int>(type: "int", nullable: true),
                    KundenGruppeWertId = table.Column<int>(type: "int", nullable: true),
                    Limit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    PreisAusblenden = table.Column<bool>(type: "bit", nullable: false),
                    ReceivableSource = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    LiefertMo = table.Column<bool>(type: "bit", nullable: false),
                    LiefertDi = table.Column<bool>(type: "bit", nullable: false),
                    LiefertMi = table.Column<bool>(type: "bit", nullable: false),
                    LiefertDo = table.Column<bool>(type: "bit", nullable: false),
                    LiefertFr = table.Column<bool>(type: "bit", nullable: false),
                    LiefertSa = table.Column<bool>(type: "bit", nullable: false),
                    LiefertSo = table.Column<bool>(type: "bit", nullable: false),
                    Geraete1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Geraete2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Geraete3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Geraete4 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Geraete5 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Offen = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notizen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_ProductAttributeValue_AusnahmeTurWertId",
                        column: x => x.AusnahmeTurWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_ProductAttributeValue_KundenGruppeWertId",
                        column: x => x.KundenGruppeWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_ProductAttributeValue_TurWertId",
                        column: x => x.TurWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    AttributId = table.Column<int>(type: "int", nullable: false),
                    AttributWertId = table.Column<int>(type: "int", nullable: true),
                    FreierText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAttributeMapping_ProductAttributeValue_AttributWertId",
                        column: x => x.AttributWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductAttributeMapping_ProductAttribute_AttributId",
                        column: x => x.AttributId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductAttributeMapping_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Gewicht = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProduct_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rechnungsnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    Rechnungsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Von = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Bis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BelegArt = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    OriginalRechnungId = table.Column<int>(type: "int", nullable: true),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IstSammelrechnung = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Gesamtnetto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtmwst = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtbrutto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Invoices_OriginalRechnungId",
                        column: x => x.OriginalRechnungId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Auftragsnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    LieferDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gesamtnetto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtmwst = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtbrutto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerProductAttributeMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProductId = table.Column<int>(type: "int", nullable: false),
                    AttributId = table.Column<int>(type: "int", nullable: false),
                    AttributWertId = table.Column<int>(type: "int", nullable: true),
                    FreierText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerProductAttributeMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_CustomerProduct_CustomerProductId",
                        column: x => x.CustomerProductId,
                        principalTable: "CustomerProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_ProductAttributeValue_AttributWertId",
                        column: x => x.AttributWertId,
                        principalTable: "ProductAttributeValue",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProductAttributeMapping_ProductAttribute_AttributId",
                        column: x => x.AttributId,
                        principalTable: "ProductAttribute",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lieferscheinnummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    AuftragId = table.Column<int>(type: "int", nullable: true),
                    LieferDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Gesamtnetto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtmwst = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Gesamtbrutto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_Customer_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Orders_AuftragId",
                        column: x => x.AuftragId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuftragId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    MengeGeliefert = table.Column<decimal>(type: "decimal(10,3)", nullable: false, defaultValue: 0m),
                    MengeFakturiert = table.Column<decimal>(type: "decimal(10,3)", nullable: false, defaultValue: 0m),
                    Gewicht = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    DiscountProzent = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 7.00m),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AmountInclVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_AuftragId",
                        column: x => x.AuftragId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LieferscheinId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    MengeGeliefert = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Gewicht = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AuftragZeileId = table.Column<int>(type: "int", nullable: true),
                    MengeFakturiert = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    DiscountProzent = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 7.00m),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AmountInclVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryLines_Deliveries_LieferscheinId",
                        column: x => x.LieferscheinId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryLines_OrderLines_AuftragZeileId",
                        column: x => x.AuftragZeileId,
                        principalTable: "OrderLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveryLines_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RechnungId = table.Column<int>(type: "int", nullable: false),
                    LieferscheinId = table.Column<int>(type: "int", nullable: false),
                    DeliveryLineId = table.Column<int>(type: "int", nullable: false),
                    ArtikelId = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    FakturierteMenge = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    Preis = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    MwstProzent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 7.00m),
                    Gewicht = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false, defaultValue: 0m),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    DiscountProzent = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    LineAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    AmountInclVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Notiz = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ErstelltVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GeaendertAm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeaendertVon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Deliveries_LieferscheinId",
                        column: x => x.LieferscheinId,
                        principalTable: "Deliveries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceLines_DeliveryLines_DeliveryLineId",
                        column: x => x.DeliveryLineId,
                        principalTable: "DeliveryLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Invoices_RechnungId",
                        column: x => x.RechnungId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Product_ArtikelId",
                        column: x => x.ArtikelId,
                        principalTable: "Product",
                        principalColumn: "Id");
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
                name: "IX_Customer_AusnahmeTurWertId",
                table: "Customer",
                column: "AusnahmeTurWertId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_KundenGruppeWertId",
                table: "Customer",
                column: "KundenGruppeWertId");

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
                name: "IX_Customer_TurWertId",
                table: "Customer",
                column: "TurWertId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProduct_ArtikelId",
                table: "CustomerProduct",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProduct_KundeId_ArtikelId",
                table: "CustomerProduct",
                columns: new[] { "KundeId", "ArtikelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_AttributId",
                table: "CustomerProductAttributeMapping",
                column: "AttributId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_AttributWertId",
                table: "CustomerProductAttributeMapping",
                column: "AttributWertId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProductAttributeMapping_CustomerProductId",
                table: "CustomerProductAttributeMapping",
                column: "CustomerProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_AuftragId",
                table: "Deliveries",
                column: "AuftragId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_KundeId",
                table: "Deliveries",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_Lieferscheinnummer",
                table: "Deliveries",
                column: "Lieferscheinnummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_ArtikelId",
                table: "DeliveryLines",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_AuftragZeileId",
                table: "DeliveryLines",
                column: "AuftragZeileId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryLines_LieferscheinId",
                table: "DeliveryLines",
                column: "LieferscheinId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver_Fahrercode",
                table: "Driver",
                column: "Fahrercode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_ArtikelId",
                table: "InvoiceLines",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_DeliveryLineId",
                table: "InvoiceLines",
                column: "DeliveryLineId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_LieferscheinId",
                table: "InvoiceLines",
                column: "LieferscheinId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_RechnungId",
                table: "InvoiceLines",
                column: "RechnungId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_KundeId",
                table: "Invoices",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OriginalRechnungId",
                table: "Invoices",
                column: "OriginalRechnungId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Rechnungsnummer",
                table: "Invoices",
                column: "Rechnungsnummer",
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
                name: "IX_OrderLines_ArtikelId",
                table: "OrderLines",
                column: "ArtikelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_AuftragId",
                table: "OrderLines",
                column: "AuftragId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Auftragsnummer",
                table: "Orders",
                column: "Auftragsnummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KundeId_LieferDatum",
                table: "Orders",
                columns: new[] { "KundeId", "LieferDatum" },
                unique: true,
                filter: "[Status] <> 3 AND [Status] <> 4");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentLines_PaymentHeaderId",
                table: "PaymentLines",
                column: "PaymentHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Artikelnummer",
                table: "Product",
                column: "Artikelnummer",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttribute_Bezeichnung",
                table: "ProductAttribute",
                column: "Bezeichnung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMapping_ArtikelId_AttributId",
                table: "ProductAttributeMapping",
                columns: new[] { "ArtikelId", "AttributId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMapping_AttributId",
                table: "ProductAttributeMapping",
                column: "AttributId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeMapping_AttributWertId",
                table: "ProductAttributeMapping",
                column: "AttributWertId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValue_AttributId",
                table: "ProductAttributeValue",
                column: "AttributId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_Routencode",
                table: "Route",
                column: "Routencode",
                unique: true);

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
                name: "AppSetup");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "CustomerProductAttributeMapping");

            migrationBuilder.DropTable(
                name: "Driver");

            migrationBuilder.DropTable(
                name: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "NoSeries");

            migrationBuilder.DropTable(
                name: "PaymentLines");

            migrationBuilder.DropTable(
                name: "ProductAttributeMapping");

            migrationBuilder.DropTable(
                name: "UserGridSetting");

            migrationBuilder.DropTable(
                name: "CustomerProduct");

            migrationBuilder.DropTable(
                name: "DeliveryLines");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "PaymentHeaders");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ProductAttributeValue");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "ProductAttribute");
        }
    }
}
