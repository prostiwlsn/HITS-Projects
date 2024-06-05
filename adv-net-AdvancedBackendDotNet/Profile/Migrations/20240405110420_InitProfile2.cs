using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Migrations
{
    /// <inheritdoc />
    public partial class InitProfile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Document_EducationDocumentUserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Document_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Document");

            migrationBuilder.CreateTable(
                name: "EducationDocuments",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationDocuments", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_EducationDocuments_Document_UserId",
                        column: x => x.UserId,
                        principalTable: "Document",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Passports_Document_UserId",
                        column: x => x.UserId,
                        principalTable: "Document",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_EducationDocuments_EducationDocumentUserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Passports_PassportUserId",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "EducationDocuments");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Document",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Document_EducationDocumentUserId",
                table: "Profiles",
                column: "EducationDocumentUserId",
                principalTable: "Document",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Document_PassportUserId",
                table: "Profiles",
                column: "PassportUserId",
                principalTable: "Document",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
