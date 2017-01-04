using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddDocumentTaskTemplateAndTypeAndPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTaskTemplates",
                columns: table => new
                {
                    IdDocumentTaskTemplate = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTemplate = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTaskTemplates", x => x.IdDocumentTaskTemplate);
                    table.ForeignKey(
                        name: "FK_DocumentTaskTemplates_DocumentTemplates_IdDocumentTemplate",
                        column: x => x.IdDocumentTemplate,
                        principalTable: "DocumentTemplates",
                        principalColumn: "IdDocumentTemplate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTaskTypes",
                columns: table => new
                {
                    IdDocumentTaskType = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTaskTemplate = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTaskTypes", x => x.IdDocumentTaskType);
                    table.ForeignKey(
                        name: "FK_DocumentTaskTypes_DocumentTaskTemplates_IdDocumentTaskTemplate",
                        column: x => x.IdDocumentTaskTemplate,
                        principalTable: "DocumentTaskTemplates",
                        principalColumn: "IdDocumentTaskTemplate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTaskTypePaths",
                columns: table => new
                {
                    IdDocumentTaskTypePath = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTaskType = table.Column<int>(nullable: false),
                    IdNextPath = table.Column<int>(nullable: true),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTaskTypePaths", x => x.IdDocumentTaskTypePath);
                    table.ForeignKey(
                        name: "FK_DocumentTaskTypePaths_DocumentTaskTypes_IdDocumentTaskType",
                        column: x => x.IdDocumentTaskType,
                        principalTable: "DocumentTaskTypes",
                        principalColumn: "IdDocumentTaskType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentTaskTypePaths_DocumentTaskTypePaths_IdNextPath",
                        column: x => x.IdNextPath,
                        principalTable: "DocumentTaskTypePaths",
                        principalColumn: "IdDocumentTaskTypePath",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskTemplates_IdDocumentTemplate",
                table: "DocumentTaskTemplates",
                column: "IdDocumentTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskTypes_IdDocumentTaskTemplate",
                table: "DocumentTaskTypes",
                column: "IdDocumentTaskTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskTypePaths_IdDocumentTaskType",
                table: "DocumentTaskTypePaths",
                column: "IdDocumentTaskType");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTaskTypePaths_IdNextPath",
                table: "DocumentTaskTypePaths",
                column: "IdNextPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTaskTypePaths");

            migrationBuilder.DropTable(
                name: "DocumentTaskTypes");

            migrationBuilder.DropTable(
                name: "DocumentTaskTemplates");
        }
    }
}
