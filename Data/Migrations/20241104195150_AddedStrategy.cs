using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrategyId",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Diversifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diversifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hedges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PositionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScalingOut = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ScalingIn = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AverageLevel = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskManagements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    BaseRiskPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RiskToRewardRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HedgeId = table.Column<int>(type: "int", nullable: false),
                    DiversificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskManagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskManagements_Diversifications_DiversificationId",
                        column: x => x.DiversificationId,
                        principalTable: "Diversifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskManagements_Hedges_HedgeId",
                        column: x => x.HedgeId,
                        principalTable: "Hedges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Strategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskManagementId = table.Column<int>(type: "int", nullable: false),
                    PositionManagementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Strategies_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Strategies_RiskManagements_RiskManagementId",
                        column: x => x.RiskManagementId,
                        principalTable: "RiskManagements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AppUserId1",
                table: "Assets",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_StrategyId",
                table: "Assets",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskManagements_DiversificationId",
                table: "RiskManagements",
                column: "DiversificationId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskManagements_HedgeId",
                table: "RiskManagements",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_PositionManagementId",
                table: "Strategies",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_RiskManagementId",
                table: "Strategies",
                column: "RiskManagementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId1",
                table: "Assets",
                column: "AppUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Strategies_StrategyId",
                table: "Assets",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId1",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Strategies_StrategyId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "Strategies");

            migrationBuilder.DropTable(
                name: "PositionEntries");

            migrationBuilder.DropTable(
                name: "RiskManagements");

            migrationBuilder.DropTable(
                name: "Diversifications");

            migrationBuilder.DropTable(
                name: "Hedges");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AppUserId1",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_StrategyId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "StrategyId",
                table: "Assets");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
