using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddUserGroupToDocumentTaskTypePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUserGroup",
                table: "DocumentTaskTypePaths",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskTypePaths_IdUserGroup",
                table: "DocumentTaskTypePaths",
                column: "IdUserGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTaskTypePaths_UserGroups_IdUserGroup",
                table: "DocumentTaskTypePaths",
                column: "IdUserGroup",
                principalTable: "UserGroups",
                principalColumn: "IdUserGroup",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTaskTypePaths_UserGroups_IdUserGroup",
                table: "DocumentTaskTypePaths");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTaskTypePaths_IdUserGroup",
                table: "DocumentTaskTypePaths");

            migrationBuilder.DropColumn(
                name: "IdUserGroup",
                table: "DocumentTaskTypePaths");
        }
    }
}
