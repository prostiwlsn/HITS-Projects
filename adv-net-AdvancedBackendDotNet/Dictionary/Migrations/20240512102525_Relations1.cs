﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dictionary.Migrations
{
    /// <inheritdoc />
    public partial class Relations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationLevels_DocumentTypes_DocumentTypeId",
                table: "EducationLevels");

            migrationBuilder.DropIndex(
                name: "IX_EducationLevels_DocumentTypeId",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "EducationLevels");

            migrationBuilder.CreateTable(
                name: "DocumentTypeEducationLevel",
                columns: table => new
                {
                    DocumentTypesId = table.Column<Guid>(type: "uuid", nullable: false),
                    NextEducationLevelsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypeEducationLevel", x => new { x.DocumentTypesId, x.NextEducationLevelsId });
                    table.ForeignKey(
                        name: "FK_DocumentTypeEducationLevel_DocumentTypes_DocumentTypesId",
                        column: x => x.DocumentTypesId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentTypeEducationLevel_EducationLevels_NextEducationLev~",
                        column: x => x.NextEducationLevelsId,
                        principalTable: "EducationLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypeEducationLevel_NextEducationLevelsId",
                table: "DocumentTypeEducationLevel",
                column: "NextEducationLevelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTypeEducationLevel");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "EducationLevels",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationLevels_DocumentTypeId",
                table: "EducationLevels",
                column: "DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationLevels_DocumentTypes_DocumentTypeId",
                table: "EducationLevels",
                column: "DocumentTypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id");
        }
    }
}
