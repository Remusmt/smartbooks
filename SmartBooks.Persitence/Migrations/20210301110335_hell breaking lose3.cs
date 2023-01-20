using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class hellbreakinglose3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_SubLedgerBases_MemberId",
                table: "TransactionItems");

            migrationBuilder.DropTable(
                name: "MemberAccountTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_MemberId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "MessageToMember",
                table: "TransactionItems");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "TransactionItems",
                newName: "SaccoFeesType");

            migrationBuilder.RenameColumn(
                name: "ApprovalAction",
                table: "TransactionItems",
                newName: "PenaltyAccountId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "TransactionItems",
                newName: "PaymentItemType");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumberPrefix",
                table: "TransactionItems",
                type: "varchar(150) CHARACTER SET utf8mb4",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TransactionItems",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AutoGenerateAccountNumbers",
                table: "TransactionItems",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryName",
                table: "TransactionItems",
                type: "varchar(150) CHARACTER SET utf8mb4",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryPhoneNo",
                table: "TransactionItems",
                type: "varchar(150) CHARACTER SET utf8mb4",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterestAccountId",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "TransactionItems",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InterestType",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LedgerAccountId",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAccountLength",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinConsecutiveContribution",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextAccountNumber",
                table: "TransactionItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    ApprovalAction = table.Column<int>(type: "int", nullable: false),
                    MessageToMember = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Comments = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
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
                    table.PrimaryKey("PK_MemberApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberApprovals_SubLedgerBases_MemberId",
                        column: x => x.MemberId,
                        principalTable: "SubLedgerBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_InterestAccountId",
                table: "TransactionItems",
                column: "InterestAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_LedgerAccountId",
                table: "TransactionItems",
                column: "LedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_PenaltyAccountId",
                table: "TransactionItems",
                column: "PenaltyAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberApprovals_MemberId",
                table: "MemberApprovals",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_InterestAccountId",
                table: "TransactionItems",
                column: "InterestAccountId",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_LedgerAccountId",
                table: "TransactionItems",
                column: "LedgerAccountId",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_PenaltyAccountId",
                table: "TransactionItems",
                column: "PenaltyAccountId",
                principalTable: "LedgerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_InterestAccountId",
                table: "TransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_LedgerAccountId",
                table: "TransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItems_LedgerAccounts_PenaltyAccountId",
                table: "TransactionItems");

            migrationBuilder.DropTable(
                name: "MemberApprovals");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_InterestAccountId",
                table: "TransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_LedgerAccountId",
                table: "TransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItems_PenaltyAccountId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "AccountNumberPrefix",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "AutoGenerateAccountNumbers",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "BeneficiaryName",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "BeneficiaryPhoneNo",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "InterestAccountId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "InterestType",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "LedgerAccountId",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "MaxAccountLength",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "MinConsecutiveContribution",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "NextAccountNumber",
                table: "TransactionItems");

            migrationBuilder.RenameColumn(
                name: "SaccoFeesType",
                table: "TransactionItems",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "PenaltyAccountId",
                table: "TransactionItems",
                newName: "ApprovalAction");

            migrationBuilder.RenameColumn(
                name: "PaymentItemType",
                table: "TransactionItems",
                newName: "ApplicationUserId");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "TransactionItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageToMember",
                table: "TransactionItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemberAccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Code = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    Discriminator = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LedgerAccountId = table.Column<int>(type: "int", nullable: false),
                    PaymentItemType = table.Column<int>(type: "int", nullable: false),
                    SystemGenerated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false),
                    AccountNumberPrefix = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    AutoGenerateAccountNumbers = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    InterestAccountId = table.Column<int>(type: "int", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    InterestType = table.Column<int>(type: "int", nullable: true),
                    MaxAccountLength = table.Column<int>(type: "int", nullable: true),
                    MinConsecutiveContribution = table.Column<int>(type: "int", nullable: true),
                    NextAccountNumber = table.Column<int>(type: "int", nullable: true),
                    PenaltyAccountId = table.Column<int>(type: "int", nullable: true),
                    SaccoFeesType = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true),
                    BeneficiaryPhoneNo = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAccountTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberAccountTypes_LedgerAccounts_InterestAccountId",
                        column: x => x.InterestAccountId,
                        principalTable: "LedgerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberAccountTypes_LedgerAccounts_LedgerAccountId",
                        column: x => x.LedgerAccountId,
                        principalTable: "LedgerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberAccountTypes_LedgerAccounts_PenaltyAccountId",
                        column: x => x.PenaltyAccountId,
                        principalTable: "LedgerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_MemberId",
                table: "TransactionItems",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccountTypes_InterestAccountId",
                table: "MemberAccountTypes",
                column: "InterestAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccountTypes_LedgerAccountId",
                table: "MemberAccountTypes",
                column: "LedgerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAccountTypes_PenaltyAccountId",
                table: "MemberAccountTypes",
                column: "PenaltyAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItems_SubLedgerBases_MemberId",
                table: "TransactionItems",
                column: "MemberId",
                principalTable: "SubLedgerBases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
