using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProiectColectiv.Services.Migrations
{
    public partial class RemoveDocumentStatesAndAddDocumentData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "DocumentStates");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "DocumentStates");

            migrationBuilder.DropTable(
                name: "DocumentTemplateStateItems");

            migrationBuilder.CreateTable(
                name: "DocumentDatas",
                columns: table => new
                {
                    IdDocumentData = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDatas", x => x.IdDocumentData);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDataTemplateItems",
                columns: table => new
                {
                    IdDocumentDataTemplateItem = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocumentDataTemplateIdDocumentData = table.Column<int>(nullable: true),
                    IdDocumentData = table.Column<int>(nullable: false),
                    IdDocumentTemplateItem = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDataTemplateItems", x => x.IdDocumentDataTemplateItem);
                    table.ForeignKey(
                        name: "FK_DocumentDataTemplateItems_DocumentDatas_DocumentDataTemplateIdDocumentData",
                        column: x => x.DocumentDataTemplateIdDocumentData,
                        principalTable: "DocumentDatas",
                        principalColumn: "IdDocumentData",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentDataTemplateItems_DocumentDatas_IdDocumentData",
                        column: x => x.IdDocumentData,
                        principalTable: "DocumentDatas",
                        principalColumn: "IdDocumentData",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentDataTemplateItems_DocumentTemplateItems_IdDocumentTemplateItem",
                        column: x => x.IdDocumentTemplateItem,
                        principalTable: "DocumentTemplateItems",
                        principalColumn: "IdDocumentTemplateItem",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdDocumentData",
                table: "DocumentStates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentStates_IdDocumentData",
                table: "DocumentStates",
                column: "IdDocumentData");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataTemplateItems_DocumentDataTemplateIdDocumentData",
                table: "DocumentDataTemplateItems",
                column: "DocumentDataTemplateIdDocumentData");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataTemplateItems_IdDocumentData",
                table: "DocumentDataTemplateItems",
                column: "IdDocumentData");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentDataTemplateItems_IdDocumentTemplateItem",
                table: "DocumentDataTemplateItems",
                column: "IdDocumentTemplateItem");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentStates_DocumentDatas_IdDocumentData",
                table: "DocumentStates",
                column: "IdDocumentData",
                principalTable: "DocumentDatas",
                principalColumn: "IdDocumentData",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentStates_DocumentDatas_IdDocumentData",
                table: "DocumentStates");

            migrationBuilder.DropIndex(
                name: "IX_DocumentStates_IdDocumentData",
                table: "DocumentStates");

            migrationBuilder.DropColumn(
                name: "IdDocumentData",
                table: "DocumentStates");

            migrationBuilder.DropTable(
                name: "DocumentDataTemplateItems");

            migrationBuilder.DropTable(
                name: "DocumentDatas");

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
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "DocumentStates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateStateItems_DocumentTemplateStateIdDocumentState",
                table: "DocumentTemplateStateItems",
                column: "DocumentTemplateStateIdDocumentState");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTemplateStateItems_IdDocumentTemplateItem",
                table: "DocumentTemplateStateItems",
                column: "IdDocumentTemplateItem");
        }
    }
}
