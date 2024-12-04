using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTargetLevelToPositionManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AverageLevel = table.Column<double>(type: "float", nullable: true),
                    PositionManagementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseLevel_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AverageLevel = table.Column<double>(type: "float", nullable: true),
                    PositionManagementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesLevels_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLevel_PositionManagementId",
                table: "PurchaseLevel",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLevels_PositionManagementId",
                table: "SalesLevels",
                column: "PositionManagementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseLevel");

            migrationBuilder.DropTable(
                name: "SalesLevels");
        }
    }
}
