using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Migrations
{
    /// <inheritdoc />
    public partial class InitProfile5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentUserId",
                table: "Files",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "EducationDocumentUserId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_EducationDocumentUserId",
                table: "Files",
                column: "EducationDocumentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_EducationDocuments_EducationDocumentUserId",
                table: "Files",
                column: "EducationDocumentUserId",
                principalTable: "EducationDocuments",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Passports_DocumentUserId",
                table: "Files",
                column: "DocumentUserId",
                principalTable: "Passports",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_EducationDocuments_EducationDocumentUserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Passports_DocumentUserId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_EducationDocumentUserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "EducationDocumentUserId",
                table: "Files");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentUserId",
                table: "Files",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
