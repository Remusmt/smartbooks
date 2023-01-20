using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBooks.Persitence.Migrations
{
    public partial class saccoentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "SubLedgerBases",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "SubLedgerBases",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeAddressId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "IndentificationNo",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LearntAboutUs",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaritalStatus",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberNumber",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberStatus",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Member_Gender",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NearestTown",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OccupationType",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherNames",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
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
                name: "PermanentAddressId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Shared",
                table: "SubLedgerBases",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "SubLedgerBases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SubLedgerBases",
                type: "longtext CHARACTER SET utf8mb4",
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

            migrationBuilder.CreateTable(
                name: "MemberAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberAttachments_SubLedgerBases_MemberId",
                        column: x => x.MemberId,
                        principalTable: "SubLedgerBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NextOfKins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Relation = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Contacts = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsMinor = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CareOf = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UpdateCode = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByName = table.Column<string>(type: "varchar(150) CHARACTER SET utf8mb4", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextOfKins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NextOfKins_SubLedgerBases_MemberId",
                        column: x => x.MemberId,
                        principalTable: "SubLedgerBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBases_ApplicationUserId",
                table: "SubLedgerBases",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBases_HomeAddressId",
                table: "SubLedgerBases",
                column: "HomeAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBases_PermanentAddressId",
                table: "SubLedgerBases",
                column: "PermanentAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberApprovals_MemberId",
                table: "MemberApprovals",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAttachments_AttachmentId",
                table: "MemberAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAttachments_MemberId",
                table: "MemberAttachments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NextOfKins_MemberId",
                table: "NextOfKins",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubLedgerBases_Addresses_HomeAddressId",
                table: "SubLedgerBases",
                column: "HomeAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubLedgerBases_Addresses_PermanentAddressId",
                table: "SubLedgerBases",
                column: "PermanentAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubLedgerBases_ApplicationUsers_ApplicationUserId",
                table: "SubLedgerBases",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubLedgerBases_Addresses_HomeAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropForeignKey(
                name: "FK_SubLedgerBases_Addresses_PermanentAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropForeignKey(
                name: "FK_SubLedgerBases_ApplicationUsers_ApplicationUserId",
                table: "SubLedgerBases");

            migrationBuilder.DropTable(
                name: "MemberApprovals");

            migrationBuilder.DropTable(
                name: "MemberAttachments");

            migrationBuilder.DropTable(
                name: "NextOfKins");

            migrationBuilder.DropIndex(
                name: "IX_SubLedgerBases_ApplicationUserId",
                table: "SubLedgerBases");

            migrationBuilder.DropIndex(
                name: "IX_SubLedgerBases_HomeAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropIndex(
                name: "IX_SubLedgerBases_PermanentAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "HomeAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "IdBackAttachmentId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "IdFrontAttachmentId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "IndentificationNo",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "LearntAboutUs",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "MemberNumber",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "MemberStatus",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Member_Gender",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "NearestTown",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "OccupationType",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "OtherNames",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "PassportCopyId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "PassportPhotoId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "PermanentAddressId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Shared",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "SubLedgerBases");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SubLedgerBases");
        }
    }
}
