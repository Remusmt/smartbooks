using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class TransactionDetailsItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_TransactionItems_InventoryItemId",
                table: "TransactionDetails");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "TransactionDetails",
                newName: "TransactionItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetails_InventoryItemId",
                table: "TransactionDetails",
                newName: "IX_TransactionDetails_TransactionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_TransactionItems_TransactionItemId",
                table: "TransactionDetails",
                column: "TransactionItemId",
                principalTable: "TransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_TransactionItems_TransactionItemId",
                table: "TransactionDetails");

            migrationBuilder.RenameColumn(
                name: "TransactionItemId",
                table: "TransactionDetails",
                newName: "InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetails_TransactionItemId",
                table: "TransactionDetails",
                newName: "IX_TransactionDetails_InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_TransactionItems_InventoryItemId",
                table: "TransactionDetails",
                column: "InventoryItemId",
                principalTable: "TransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
