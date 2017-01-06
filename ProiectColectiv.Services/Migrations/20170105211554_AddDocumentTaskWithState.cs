using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddDocumentTaskWithState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTasks",
                columns: table => new
                {
                    IdDocumentTask = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTaskType = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTasks", x => x.IdDocumentTask);
                    table.ForeignKey(
                        name: "FK_DocumentTasks_DocumentTaskTypes_IdDocumentTaskType",
                        column: x => x.IdDocumentTaskType,
                        principalTable: "DocumentTaskTypes",
                        principalColumn: "IdDocumentTaskType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTaskStates",
                columns: table => new
                {
                    IdDocumentTaskState = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentTaskStatus = table.Column<int>(nullable: false),
                    IdDocumentTaskTypePath = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTaskStates", x => x.IdDocumentTaskState);
                    table.ForeignKey(
                        name: "FK_DocumentTaskStates_DocumentTaskTypePaths_IdDocumentTaskTypePath",
                        column: x => x.IdDocumentTaskTypePath,
                        principalTable: "DocumentTaskTypePaths",
                        principalColumn: "IdDocumentTaskTypePath",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdDocumentTask",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_IdDocumentTask",
                table: "Documents",
                column: "IdDocumentTask");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTasks_IdDocumentTaskType",
                table: "DocumentTasks",
                column: "IdDocumentTaskType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTasks_UserId",
                table: "DocumentTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskStates_IdDocumentTaskTypePath",
                table: "DocumentTaskStates",
                column: "IdDocumentTaskTypePath");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTasks_IdDocumentTask",
                table: "Documents",
                column: "IdDocumentTask",
                principalTable: "DocumentTasks",
                principalColumn: "IdDocumentTask",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTasks_IdDocumentTask",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_IdDocumentTask",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "IdDocumentTask",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentTasks");

            migrationBuilder.DropTable(
                name: "DocumentTaskStates");
        }
    }
}
