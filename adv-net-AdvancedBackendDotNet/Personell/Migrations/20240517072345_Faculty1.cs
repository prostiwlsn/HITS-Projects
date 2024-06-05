using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personell.Migrations
{
    /// <inheritdoc />
    public partial class Faculty1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacultyName",
                table: "Personell",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacultyName",
                table: "Personell");
        }
    }
}
