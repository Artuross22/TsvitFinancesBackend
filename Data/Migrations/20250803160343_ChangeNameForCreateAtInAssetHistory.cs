using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameForCreateAtInAssetHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "AssetHistory",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "AssetId1",
                table: "AssetHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_AssetId1",
                table: "AssetHistory",
                column: "AssetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetHistory_Assets_AssetId1",
                table: "AssetHistory",
                column: "AssetId1",
                principalTable: "Assets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetHistory_Assets_AssetId1",
                table: "AssetHistory");

            migrationBuilder.DropIndex(
                name: "IX_AssetHistory_AssetId1",
                table: "AssetHistory");

            migrationBuilder.DropColumn(
                name: "AssetId1",
                table: "AssetHistory");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AssetHistory",
                newName: "CreateAt");
        }
    }
}
