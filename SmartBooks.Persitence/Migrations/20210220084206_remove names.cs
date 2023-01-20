using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class removenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherNames",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "SubLedgerBases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherNames",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
