using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class defaultcodelength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultCodeLength",
                table: "CompanyDefaults",
                type: "int",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.CreateTable(
                name: "SingleValue",
                columns: table => new
                {
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleValue");

            migrationBuilder.DropColumn(
                name: "DefaultCodeLength",
                table: "CompanyDefaults");
        }
    }
}
