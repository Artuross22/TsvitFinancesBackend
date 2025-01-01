using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionEntryNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Assets_AssetId",
                table: "Charts");

            migrationBuilder.DropIndex(
                name: "IX_Charts_AssetId",
                table: "Charts");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Charts");

            migrationBuilder.AddColumn<int>(
                name: "PositionEntryNoteId",
                table: "Charts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PositionEntryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionEntryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionEntryNotes_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_PositionEntryNoteId",
                table: "Charts",
                column: "PositionEntryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionEntryNotes_AssetId",
                table: "PositionEntryNotes",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts",
                column: "PositionEntryNoteId",
                principalTable: "PositionEntryNotes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts");

            migrationBuilder.DropTable(
                name: "PositionEntryNotes");

            migrationBuilder.DropIndex(
                name: "IX_Charts_PositionEntryNoteId",
                table: "Charts");

            migrationBuilder.DropColumn(
                name: "PositionEntryNoteId",
                table: "Charts");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "Charts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Charts_AssetId",
                table: "Charts",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Assets_AssetId",
                table: "Charts",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
