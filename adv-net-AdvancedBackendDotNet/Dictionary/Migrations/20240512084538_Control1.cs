using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dictionary.Migrations
{
    /// <inheritdoc />
    public partial class Control1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationProgram_EducationLevels_EducationLevelId",
                table: "EducationProgram");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationProgram_Faculties_FacultyId",
                table: "EducationProgram");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EducationProgram",
                table: "EducationProgram");

            migrationBuilder.RenameTable(
                name: "EducationProgram",
                newName: "Programs");

            migrationBuilder.RenameIndex(
                name: "IX_EducationProgram_FacultyId",
                table: "Programs",
                newName: "IX_Programs_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_EducationProgram_EducationLevelId",
                table: "Programs",
                newName: "IX_Programs_EducationLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Programs",
                table: "Programs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_EducationLevels_EducationLevelId",
                table: "Programs",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Faculties_FacultyId",
                table: "Programs",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_EducationLevels_EducationLevelId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Faculties_FacultyId",
                table: "Programs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Programs",
                table: "Programs");

            migrationBuilder.RenameTable(
                name: "Programs",
                newName: "EducationProgram");

            migrationBuilder.RenameIndex(
                name: "IX_Programs_FacultyId",
                table: "EducationProgram",
                newName: "IX_EducationProgram_FacultyId");

            migrationBuilder.RenameIndex(
                name: "IX_Programs_EducationLevelId",
                table: "EducationProgram",
                newName: "IX_EducationProgram_EducationLevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EducationProgram",
                table: "EducationProgram",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationProgram_EducationLevels_EducationLevelId",
                table: "EducationProgram",
                column: "EducationLevelId",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationProgram_Faculties_FacultyId",
                table: "EducationProgram",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
