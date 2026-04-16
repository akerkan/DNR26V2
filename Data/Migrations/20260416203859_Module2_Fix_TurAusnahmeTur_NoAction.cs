using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DNR26V2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Module2_Fix_TurAusnahmeTur_NoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerFilter_KundenfilterId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_RouteId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "Wochenendtour",
                table: "Customer",
                newName: "LiefertSo");

            migrationBuilder.AlterColumn<int>(
                name: "Routenfolge",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Notizen",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Limit",
                table: "Customer",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete5",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete4",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete3",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete2",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete1",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adresse2",
                table: "Customer",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adresse",
                table: "Customer",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALAdresse2",
                table: "Customer",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALAdresse",
                table: "Customer",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AusnahmeTurId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerFilterId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertDi",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertDo",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertFr",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertMi",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertMo",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LiefertSa",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TurId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AusnahmeTurId",
                table: "Customer",
                column: "AusnahmeTurId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerFilterId",
                table: "Customer",
                column: "CustomerFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TurId",
                table: "Customer",
                column: "TurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerFilter_CustomerFilterId",
                table: "Customer",
                column: "CustomerFilterId",
                principalTable: "CustomerFilter",
                principalColumn: "Id");

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
                name: "FK_Customer_Route_RouteId",
                table: "Customer",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Route_TurId",
                table: "Customer",
                column: "TurId",
                principalTable: "Route",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerFilter_CustomerFilterId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerFilter_KundenfilterId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_AusnahmeTurId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_RouteId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Route_TurId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_AusnahmeTurId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerFilterId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_TurId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AusnahmeTurId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerFilterId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertDi",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertDo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertFr",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertMi",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertMo",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LiefertSa",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TurId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "LiefertSo",
                table: "Customer",
                newName: "Wochenendtour");

            migrationBuilder.AlterColumn<int>(
                name: "Routenfolge",
                table: "Customer",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Notizen",
                table: "Customer",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Limit",
                table: "Customer",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete5",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete4",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete3",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete2",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Geraete1",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adresse2",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adresse",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALAdresse2",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALAdresse",
                table: "Customer",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerFilter_KundenfilterId",
                table: "Customer",
                column: "KundenfilterId",
                principalTable: "CustomerFilter",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Route_RouteId",
                table: "Customer",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
