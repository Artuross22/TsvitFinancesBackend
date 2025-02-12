using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionScaling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AppUsers");

            migrationBuilder.CreateTable(
                name: "PositionScalings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquityPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PositionType = table.Column<int>(type: "int", nullable: false),
                    PositionManagementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionScalings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionScalings_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionScalings_PositionManagementId",
                table: "PositionScalings",
                column: "PositionManagementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PositionScalings");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
