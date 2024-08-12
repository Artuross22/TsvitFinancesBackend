using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserToAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sector",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AppUserId",
                table: "Assets",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_AppUsers_AppUserId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AppUserId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AppUsers");
        }
    }
}
