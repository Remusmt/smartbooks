using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class renameitemtodetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralLedgers_TransactionItems_TransactionItemId",
                table: "GeneralLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_TransactionItems_TransactionItemId",
                table: "JournalDetails");

            migrationBuilder.DropTable(
                name: "TransactionItems");

            migrationBuilder.RenameColumn(
                name: "TransactionItemId",
                table: "GeneralLedgers",
                newName: "TransactionDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralLedgers_TransactionItemId",
                table: "GeneralLedgers",
                newName: "IX_GeneralLedgers_TransactionDetailId");

            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    InventoryItemId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false),
                    SystemGenerated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Categories_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_CostCenterId",
                table: "TransactionDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_InventoryItemId",
                table: "TransactionDetails",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TransactionId",
                table: "TransactionDetails",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralLedgers_TransactionDetails_TransactionDetailId",
                table: "GeneralLedgers",
                column: "TransactionDetailId",
                principalTable: "TransactionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionItemId",
                table: "JournalDetails",
                column: "TransactionItemId",
                principalTable: "TransactionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralLedgers_TransactionDetails_TransactionDetailId",
                table: "GeneralLedgers");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_TransactionDetails_TransactionItemId",
                table: "JournalDetails");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.RenameColumn(
                name: "TransactionDetailId",
                table: "GeneralLedgers",
                newName: "TransactionItemId");

            migrationBuilder.RenameIndex(
                name: "IX_GeneralLedgers_TransactionDetailId",
                table: "GeneralLedgers",
                newName: "IX_GeneralLedgers_TransactionItemId");

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    InventoryItemId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SystemGenerated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Categories_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_CostCenterId",
                table: "TransactionItems",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_InventoryItemId",
                table: "TransactionItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItems",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralLedgers_TransactionItems_TransactionItemId",
                table: "GeneralLedgers",
                column: "TransactionItemId",
                principalTable: "TransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_TransactionItems_TransactionItemId",
                table: "JournalDetails",
                column: "TransactionItemId",
                principalTable: "TransactionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
