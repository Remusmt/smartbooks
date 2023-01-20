using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class AddingFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TransactionDetails",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "MemberAccountId",
                table: "TransactionDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_MemberAccountId",
                table: "TransactionDetails",
                column: "MemberAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_MemberAccounts_MemberAccountId",
                table: "TransactionDetails",
                column: "MemberAccountId",
                principalTable: "MemberAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_MemberAccounts_MemberAccountId",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_MemberAccountId",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "MemberAccountId",
                table: "TransactionDetails");
        }
    }
}
