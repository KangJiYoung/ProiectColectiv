using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class MakeUserGroupNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserGroup",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers",
                column: "IdUserGroup",
                principalTable: "UserGroups",
                principalColumn: "IdUserGroup",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserGroup",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers",
                column: "IdUserGroup",
                principalTable: "UserGroups",
                principalColumn: "IdUserGroup",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
