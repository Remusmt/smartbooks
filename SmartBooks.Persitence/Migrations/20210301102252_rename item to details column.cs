using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class renameitemtodetailscolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionItemId",
                table: "JournalDetails");

            migrationBuilder.RenameColumn(
                name: "TransactionItemId",
                table: "JournalDetails",
                newName: "TransactionDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_JournalDetails_TransactionItemId",
                table: "JournalDetails",
                newName: "IX_JournalDetails_TransactionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionDetailId",
                table: "JournalDetails",
                column: "TransactionDetailId",
                principalTable: "TransactionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionDetailId",
                table: "JournalDetails");

            migrationBuilder.RenameColumn(
                name: "TransactionDetailId",
                table: "JournalDetails",
                newName: "TransactionItemId");

            migrationBuilder.RenameIndex(
                name: "IX_JournalDetails_TransactionDetailId",
                table: "JournalDetails",
                newName: "IX_JournalDetails_TransactionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionItemId",
                table: "JournalDetails",
                column: "TransactionItemId",
                principalTable: "TransactionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
