using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProiectColectiv.Services.Migrations
{
    public partial class AddCascadeDeleteForDocumentTemplateState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTemplateStateItems_DocumentStates_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTemplateStateItems_DocumentStates_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems",
                column: "DocumentTemplateStateIdDocumentState",
                principalTable: "DocumentStates",
                principalColumn: "IdDocumentState",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTemplateStateItems_DocumentStates_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTemplateStateItems_DocumentStates_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems",
                column: "DocumentTemplateStateIdDocumentState",
                principalTable: "DocumentStates",
                principalColumn: "IdDocumentState",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
