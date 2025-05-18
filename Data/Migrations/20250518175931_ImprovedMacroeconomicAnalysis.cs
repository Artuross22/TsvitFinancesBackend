using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ImprovedMacroeconomicAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "MacroeconomicAnalyses");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "MacroeconomicEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "MacroeconomicEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchivedAt",
                table: "MacroeconomicAnalyses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "MacroeconomicEvents");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "MacroeconomicEvents");

            migrationBuilder.DropColumn(
                name: "ArchivedAt",
                table: "MacroeconomicAnalyses");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "MacroeconomicAnalyses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
