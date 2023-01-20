using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class saccoredesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdBackAttachmentId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "IdFrontAttachmentId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "PassportCopyId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "PassportPhotoId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "MembershipFeeAmount",
                table: "SaccoSettings");

            migrationBuilder.DropColumn(
                name: "MinimumAmount",
                table: "MemberAccountTypes");

            migrationBuilder.DropColumn(
                name: "SharesItem_MinimumAmount",
                table: "MemberAccountTypes");

            migrationBuilder.RenameColumn(
                name: "OrganisationId",
                table: "Transactions",
                newName: "SubLedgerBaseId");

            migrationBuilder.RenameColumn(
                name: "MinContributionBeforeWithdraw",
                table: "MemberAccountTypes",
                newName: "SaccoFeesType");

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Warehouses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "UtilityRooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "UomConversions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "UnitofMeasures",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "TransactionItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "TeachingDepartments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "TaxRates",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Taxes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SharesContribution",
                table: "SubLedgerBases",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "SubLedgerBases",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Subjects",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "SchoolYears",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "SchoolTerms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "SaccoSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "ReorderLevels",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "PaymentTerms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "PaymentMethods",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "NextOfKins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentType",
                table: "MemberAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "MemberAttachments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "MemberApprovals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "MemberAccountTypes",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "AutoGenerateAccountNumbers",
                table: "MemberAccountTypes",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "MemberAccountTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Levels",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "LedgerEntries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "LedgerAccounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Journals",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "JournalDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "InventoryLedgers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "InventoryItems",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "GeneralLedgers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "FinancialYears",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Dormitories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "DocumentSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "CustomerProjects",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "CurrencyConversions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Currencies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Countries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "CompanyDefaults",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Companies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "ClassRooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "ClassRegisters",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "ClassRegisterDetails",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Categories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Blocks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Bins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "BinCards",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Banks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "BankBranches",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "AuditLogs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Attachments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SystemGenerated",
                table: "Addresses",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "UtilityRooms");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "UomConversions");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "UnitofMeasures");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "TeachingDepartments");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "TaxRates");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "SharesContribution",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "SchoolYears");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "SchoolTerms");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "SaccoSettings");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "ReorderLevels");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "PaymentTerms");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "NextOfKins");

            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "MemberAttachments");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "MemberAttachments");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "MemberApprovals");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MemberAccountTypes");

            migrationBuilder.DropColumn(
                name: "AutoGenerateAccountNumbers",
                table: "MemberAccountTypes");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "MemberAccountTypes");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "LedgerAccounts");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "JournalDetails");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "InventoryLedgers");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "GeneralLedgers");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "FinancialYears");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "DocumentSettings");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "CustomerProjects");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "CurrencyConversions");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "CompanyDefaults");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "ClassRooms");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "ClassRegisters");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "ClassRegisterDetails");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Bins");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "BinCards");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Banks");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "BankBranches");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "SystemGenerated",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "SubLedgerBaseId",
                table: "Transactions",
                newName: "OrganisationId");

            migrationBuilder.RenameColumn(
                name: "SaccoFeesType",
                table: "MemberAccountTypes",
                newName: "MinContributionBeforeWithdraw");

            migrationBuilder.AddColumn<int>(
                name: "IdBackAttachmentId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdFrontAttachmentId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportCopyId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassportPhotoId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MembershipFeeAmount",
                table: "SaccoSettings",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumAmount",
                table: "MemberAccountTypes",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SharesItem_MinimumAmount",
                table: "MemberAccountTypes",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);
        }
    }
}
