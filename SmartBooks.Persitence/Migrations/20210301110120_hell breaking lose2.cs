using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class hellbreakinglose2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberApprovals");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalAction",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "TransactionItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageToMember",
                table: "TransactionItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_MemberId",
                table: "TransactionItems",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_SubLedgerBases_MemberId",
                table: "TransactionItems",
                column: "MemberId",
                principalTable: "SubLedgerBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_SubLedgerBases_MemberId",
                table: "TransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_MemberId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "ApprovalAction",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "MessageToMember",
                table: "TransactionItems");

            migrationBuilder.CreateTable(
                name: "MemberApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovalAction = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    MessageToMember = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SystemGenerated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberApprovals_SubLedgerBases_MemberId",
                        column: x => x.MemberId,
                        principalTable: "SubLedgerBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberApprovals_MemberId",
                table: "MemberApprovals",
                column: "MemberId");
        }
    }
}
