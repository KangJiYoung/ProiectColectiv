using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class NamesRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriere",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "Nume",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "Descriere",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DocumentTemplates",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Documents",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DocumentTemplates");

            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Descriere",
                table: "DocumentTemplates",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nume",
                table: "DocumentTemplates",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descriere",
                table: "Documents",
                maxLength: 100,
                nullable: true);
        }
    }
}
