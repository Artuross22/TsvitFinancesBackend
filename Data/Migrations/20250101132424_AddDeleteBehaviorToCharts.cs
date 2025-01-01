using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteBehaviorToCharts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts",
                column: "PositionEntryNoteId",
                principalTable: "PositionEntryNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                table: "Charts",
                column: "PositionEntryNoteId",
                principalTable: "PositionEntryNotes",
                principalColumn: "Id");
        }
    }
}
