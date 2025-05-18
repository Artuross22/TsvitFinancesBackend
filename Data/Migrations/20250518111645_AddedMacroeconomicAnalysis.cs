using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMacroeconomicAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MacroeconomicAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EconomicType = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroeconomicAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroeconomicAnalyses_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacroeconomicEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    MacroeconomicAnalysisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroeconomicEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroeconomicEvents_MacroeconomicAnalyses_MacroeconomicAnalysisId",
                        column: x => x.MacroeconomicAnalysisId,
                        principalTable: "MacroeconomicAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MacroeconomicAnalyses_AppUserId",
                table: "MacroeconomicAnalyses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MacroeconomicEvents_MacroeconomicAnalysisId",
                table: "MacroeconomicEvents",
                column: "MacroeconomicAnalysisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacroeconomicEvents");

            migrationBuilder.DropTable(
                name: "MacroeconomicAnalyses");
        }
    }
}
