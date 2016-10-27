using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class MoveDataFromDocumentToDocumentState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Documents");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "DocumentStates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "DocumentStates");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Documents",
                nullable: true);
        }
    }
}
