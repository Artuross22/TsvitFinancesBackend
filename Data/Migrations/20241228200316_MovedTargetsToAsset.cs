using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class MovedTargetsToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseLevel_PositionEntries_PositionManagementId",
                table: "PurchaseLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesLevels_PositionEntries_PositionManagementId",
                table: "SalesLevels");

            migrationBuilder.DropIndex(
                name: "IX_SalesLevels_PositionManagementId",
                table: "SalesLevels");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseLevel_PositionManagementId",
                table: "PurchaseLevel");

            migrationBuilder.DropColumn(
                name: "PositionManagementId",
                table: "SalesLevels");

            migrationBuilder.DropColumn(
                name: "PositionManagementId",
                table: "PurchaseLevel");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "SalesLevels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "PurchaseLevel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesLevels_AssetId",
                table: "SalesLevels",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLevel_AssetId",
                table: "PurchaseLevel",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseLevel_Assets_AssetId",
                table: "PurchaseLevel",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesLevels_Assets_AssetId",
                table: "SalesLevels",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseLevel_Assets_AssetId",
                table: "PurchaseLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesLevels_Assets_AssetId",
                table: "SalesLevels");

            migrationBuilder.DropIndex(
                name: "IX_SalesLevels_AssetId",
                table: "SalesLevels");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseLevel_AssetId",
                table: "PurchaseLevel");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "SalesLevels");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "PurchaseLevel");

            migrationBuilder.AddColumn<int>(
                name: "PositionManagementId",
                table: "SalesLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionManagementId",
                table: "PurchaseLevel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalesLevels_PositionManagementId",
                table: "SalesLevels",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLevel_PositionManagementId",
                table: "PurchaseLevel",
                column: "PositionManagementId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseLevel_PositionEntries_PositionManagementId",
                table: "PurchaseLevel",
                column: "PositionManagementId",
                principalTable: "PositionEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesLevels_PositionEntries_PositionManagementId",
                table: "SalesLevels",
                column: "PositionManagementId",
                principalTable: "PositionEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
