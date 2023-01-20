using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class updatingcurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Transactions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailMessage",
                table: "Transactions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmailStatus",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DecimalMark",
                table: "Currencies",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumericCode",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubUnit",
                table: "Currencies",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUnitToUnit",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Currencies",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SymbolFirst",
                table: "Currencies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThousandSeparator",
                table: "Currencies",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EmailMessage",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EmailStatus",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DecimalMark",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "NumericCode",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SubUnit",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SubUnitToUnit",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SymbolFirst",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ThousandSeparator",
                table: "Currencies");
        }
    }
}
