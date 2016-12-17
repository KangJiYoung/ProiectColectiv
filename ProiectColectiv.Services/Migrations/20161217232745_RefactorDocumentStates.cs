using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class RefactorDocumentStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTemplateStateItems",
                columns: table => new
                {
                    IdDocumentTemplateStateItem = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentTemplateStateIdDocumentState = table.Column<int>(nullable: true),
                    IdDocumentTemplateItem = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplateStateItems", x => x.IdDocumentTemplateStateItem);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateStateItems_DocumentStates_DocumentTemplateStateIdDocumentState",
                        column: x => x.DocumentTemplateStateIdDocumentState,
                        principalTable: "DocumentStates",
                        principalColumn: "IdDocumentState",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentTemplateStateItems_DocumentTemplateItems_IdDocumentTemplateItem",
                        column: x => x.IdDocumentTemplateItem,
                        principalTable: "DocumentTemplateItems",
                        principalColumn: "IdDocumentTemplateItem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "DocumentStates",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdDocumentTemplate",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_IdDocumentTemplate",
                table: "Documents",
                column: "IdDocumentTemplate");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateStateItems_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems",
                column: "DocumentTemplateStateIdDocumentState");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateStateItems_IdDocumentTemplateItem",
                table: "DocumentTemplateStateItems",
                column: "IdDocumentTemplateItem");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_DocumentTemplates_IdDocumentTemplate",
                table: "Documents",
                column: "IdDocumentTemplate",
                principalTable: "DocumentTemplates",
                principalColumn: "IdDocumentTemplate",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_DocumentTemplates_IdDocumentTemplate",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_IdDocumentTemplate",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DocumentStates");

            migrationBuilder.DropColumn(
                name: "IdDocumentTemplate",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentTemplateStateItems");
        }
    }
}
