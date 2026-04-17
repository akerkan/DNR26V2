using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations;

public partial class Module3_MoveIstVorlageToAttribute : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // IstVorlage → ProductAttribute tablosuna taşı
        migrationBuilder.AddColumn<bool>(
            name: "IstVorlage",
            table: "ProductAttribute",
            type: "bit",
            nullable: false,
            defaultValue: false);

        // ProductAttributeValue tablosundan kaldır
        migrationBuilder.DropColumn(
            name: "IstVorlage",
            table: "ProductAttributeValue");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IstVorlage",
            table: "ProductAttribute");

        migrationBuilder.AddColumn<bool>(
            name: "IstVorlage",
            table: "ProductAttributeValue",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}