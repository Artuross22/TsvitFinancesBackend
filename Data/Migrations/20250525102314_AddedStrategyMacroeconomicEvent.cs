using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStrategyMacroeconomicEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MacroeconomicEventId",
                table: "Strategies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StrategyMacroeconomicEvents",
                columns: table => new
                {
                    StrategyId = table.Column<int>(type: "int", nullable: false),
                    MacroeconomicEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyMacroeconomicEvents", x => new { x.StrategyId, x.MacroeconomicEventId });
                    table.ForeignKey(
                        name: "FK_StrategyMacroeconomicEvents_MacroeconomicEvents_MacroeconomicEventId",
                        column: x => x.MacroeconomicEventId,
                        principalTable: "MacroeconomicEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StrategyMacroeconomicEvents_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_MacroeconomicEventId",
                table: "Strategies",
                column: "MacroeconomicEventId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyMacroeconomicEvents_MacroeconomicEventId",
                table: "StrategyMacroeconomicEvents",
                column: "MacroeconomicEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_MacroeconomicEvents_MacroeconomicEventId",
                table: "Strategies",
                column: "MacroeconomicEventId",
                principalTable: "MacroeconomicEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_MacroeconomicEvents_MacroeconomicEventId",
                table: "Strategies");

            migrationBuilder.DropTable(
                name: "StrategyMacroeconomicEvents");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_MacroeconomicEventId",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "MacroeconomicEventId",
                table: "Strategies");
        }
    }
}
