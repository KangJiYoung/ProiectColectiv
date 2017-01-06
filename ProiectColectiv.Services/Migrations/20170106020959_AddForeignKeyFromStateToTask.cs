using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddForeignKeyFromStateToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdDocumentTask",
                table: "DocumentTaskStates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskStates_IdDocumentTask",
                table: "DocumentTaskStates",
                column: "IdDocumentTask");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTaskStates_DocumentTasks_IdDocumentTask",
                table: "DocumentTaskStates",
                column: "IdDocumentTask",
                principalTable: "DocumentTasks",
                principalColumn: "IdDocumentTask",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTaskStates_DocumentTasks_IdDocumentTask",
                table: "DocumentTaskStates");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTaskStates_IdDocumentTask",
                table: "DocumentTaskStates");

            migrationBuilder.DropColumn(
                name: "IdDocumentTask",
                table: "DocumentTaskStates");
        }
    }
}
