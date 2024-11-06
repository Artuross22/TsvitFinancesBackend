using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class BalanceFlowToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "BalanceFlows",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFlows_AppUserId",
                table: "BalanceFlows",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BalanceFlows_AppUsers_AppUserId",
                table: "BalanceFlows",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_BalanceFlows_AppUsers_AppUserId",
                table: "BalanceFlows");

            migrationBuilder.DropIndex(
                name: "IX_BalanceFlows_AppUserId",
                table: "BalanceFlows");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "BalanceFlows");

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
