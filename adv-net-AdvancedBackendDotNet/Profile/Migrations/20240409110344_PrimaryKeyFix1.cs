using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Migrations
{
    /// <inheritdoc />
    public partial class PrimaryKeyFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocuments_Document_UserId",
                table: "EducationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Document_DocumentUserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Document_UserId",
                table: "Passports");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_EducationDocuments_EducationDocumentUserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Passports_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_EducationDocumentUserId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passports",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Files_DocumentUserId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationDocuments",
                table: "EducationDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationDocumentId",
                table: "Profiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PassportId",
                table: "Profiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Passports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "Files",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EducationDocuments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Document",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passports",
                table: "Passports",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationDocuments",
                table: "EducationDocuments",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_EducationDocumentId_EducationDocumentUserId",
                table: "Profiles",
                columns: new[] { "EducationDocumentId", "EducationDocumentUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PassportId_PassportUserId",
                table: "Profiles",
                columns: new[] { "PassportId", "PassportUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Files_DocumentId_DocumentUserId",
                table: "Files",
                columns: new[] { "DocumentId", "DocumentUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDocuments_Document_Id_UserId",
                table: "EducationDocuments",
                columns: new[] { "Id", "UserId" },
                principalTable: "Document",
                principalColumns: new[] { "Id", "UserId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Document_DocumentId_DocumentUserId",
                table: "Files",
                columns: new[] { "DocumentId", "DocumentUserId" },
                principalTable: "Document",
                principalColumns: new[] { "Id", "UserId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Document_Id_UserId",
                table: "Passports",
                columns: new[] { "Id", "UserId" },
                principalTable: "Document",
                principalColumns: new[] { "Id", "UserId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_EducationDocuments_EducationDocumentId_EducationDo~",
                table: "Profiles",
                columns: new[] { "EducationDocumentId", "EducationDocumentUserId" },
                principalTable: "EducationDocuments",
                principalColumns: new[] { "Id", "UserId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Passports_PassportId_PassportUserId",
                table: "Profiles",
                columns: new[] { "PassportId", "PassportUserId" },
                principalTable: "Passports",
                principalColumns: new[] { "Id", "UserId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationDocuments_Document_Id_UserId",
                table: "EducationDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Document_DocumentId_DocumentUserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Document_Id_UserId",
                table: "Passports");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_EducationDocuments_EducationDocumentId_EducationDo~",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Passports_PassportId_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_EducationDocumentId_EducationDocumentUserId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PassportId_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passports",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Files_DocumentId_DocumentUserId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationDocuments",
                table: "EducationDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "EducationDocumentId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PassportId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EducationDocuments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Document");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passports",
                table: "Passports",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationDocuments",
                table: "EducationDocuments",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_EducationDocumentUserId",
                table: "Profiles",
                column: "EducationDocumentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PassportUserId",
                table: "Profiles",
                column: "PassportUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DocumentUserId",
                table: "Files",
                column: "DocumentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationDocuments_Document_UserId",
                table: "EducationDocuments",
                column: "UserId",
                principalTable: "Document",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Document_DocumentUserId",
                table: "Files",
                column: "DocumentUserId",
                principalTable: "Document",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Document_UserId",
                table: "Passports",
                column: "UserId",
                principalTable: "Document",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_EducationDocuments_EducationDocumentUserId",
                table: "Profiles",
                column: "EducationDocumentUserId",
                principalTable: "EducationDocuments",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Passports_PassportUserId",
                table: "Profiles",
                column: "PassportUserId",
                principalTable: "Passports",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
