using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddDocumentStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentStates",
                columns: table => new
                {
                    IdDocumentState = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentStatus = table.Column<int>(nullable: false),
                    IdDocument = table.Column<int>(nullable: false),
                    StatusDate = table.Column<DateTime>(nullable: false),
                    Version = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStates", x => x.IdDocumentState);
                    table.ForeignKey(
                        name: "FK_DocumentStates_Documents_IdDocument",
                        column: x => x.IdDocument,
                        principalTable: "Documents",
                        principalColumn: "IdDocument",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentStates_IdDocument",
                table: "DocumentStates",
                column: "IdDocument");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentStates");
        }
    }
}
