using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Migrations
{
    /// <inheritdoc />
    public partial class InitProfile7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.UserId);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Document");
        }
    }
}
