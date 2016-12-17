using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddDocumentTemplateItemAndValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTemplateItems",
                columns: table => new
                {
                    IdDocumentTemplateItem = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTemplate = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateItems", x => x.IdDocumentTemplateItem);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateItems_DocumentTemplates_IdDocumentTemplate",
                        column: x => x.IdDocumentTemplate,
                        principalTable: "DocumentTemplates",
                        principalColumn: "IdDocumentTemplate",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplateItemValues",
                columns: table => new
                {
                    IdDocumentTemplateItemValue = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdDocumentTemplateItem = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateItemValues", x => x.IdDocumentTemplateItemValue);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateItemValues_DocumentTemplateItems_IdDocumentTemplateItem",
                        column: x => x.IdDocumentTemplateItem,
                        principalTable: "DocumentTemplateItems",
                        principalColumn: "IdDocumentTemplateItem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateItems_IdDocumentTemplate",
                table: "DocumentTemplateItems",
                column: "IdDocumentTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateItemValues_IdDocumentTemplateItem",
                table: "DocumentTemplateItemValues",
                column: "IdDocumentTemplateItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTemplateItemValues");

            migrationBuilder.DropTable(
                name: "DocumentTemplateItems");
        }
    }
}
