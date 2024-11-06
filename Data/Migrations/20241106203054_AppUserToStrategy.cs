using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AppUserToStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId1",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceFlows_AppUsers_AppUserId1",
                table: "BalanceFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_PositionEntries_PositionManagementId",
                table: "Strategies");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_RiskManagements_RiskManagementId",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_BalanceFlows_AppUserId1",
                table: "BalanceFlows");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AppUserId1",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BalanceFlows");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "BalanceFlows");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Assets");

            migrationBuilder.AlterColumn<int>(
                name: "RiskManagementId",
                table: "Strategies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionManagementId",
                table: "Strategies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Strategies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_AppUserId",
                table: "Strategies",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_AppUsers_AppUserId",
                table: "Strategies",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_PositionEntries_PositionManagementId",
                table: "Strategies",
                column: "PositionManagementId",
                principalTable: "PositionEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_RiskManagements_RiskManagementId",
                table: "Strategies",
                column: "RiskManagementId",
                principalTable: "RiskManagements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_AppUsers_AppUserId",
                table: "Strategies");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_PositionEntries_PositionManagementId",
                table: "Strategies");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_RiskManagements_RiskManagementId",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_AppUserId",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Strategies");

            migrationBuilder.AlterColumn<int>(
                name: "RiskManagementId",
                table: "Strategies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionManagementId",
                table: "Strategies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "BalanceFlows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "BalanceFlows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFlows_AppUserId1",
                table: "BalanceFlows",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AppUserId1",
                table: "Assets",
                column: "AppUserId1");

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
                name: "FK_BalanceFlows_AppUsers_AppUserId1",
                table: "BalanceFlows",
                column: "AppUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_PositionEntries_PositionManagementId",
                table: "Strategies",
                column: "PositionManagementId",
                principalTable: "PositionEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_RiskManagements_RiskManagementId",
                table: "Strategies",
                column: "RiskManagementId",
                principalTable: "RiskManagements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
