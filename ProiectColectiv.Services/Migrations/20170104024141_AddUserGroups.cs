using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddUserGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    IdUserGroup = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.IdUserGroup);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdUserGroup",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdUserGroup",
                table: "AspNetUsers",
                column: "IdUserGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers",
                column: "IdUserGroup",
                principalTable: "UserGroups",
                principalColumn: "IdUserGroup",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserGroups_IdUserGroup",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdUserGroup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdUserGroup",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
