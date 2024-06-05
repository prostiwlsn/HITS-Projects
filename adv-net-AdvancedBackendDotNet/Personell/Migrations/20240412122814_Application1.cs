using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personell.Migrations
{
    /// <inheritdoc />
    public partial class Application1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManagerUserId",
                table: "Applications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ManagerUserId",
                table: "Applications",
                column: "ManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Personell_ManagerUserId",
                table: "Applications",
                column: "ManagerUserId",
                principalTable: "Personell",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Personell_ManagerUserId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ManagerUserId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ManagerUserId",
                table: "Applications");
        }
    }
}
