using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentIdeaToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "InvestmentIdeas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentIdeas_AppUserId",
                table: "InvestmentIdeas",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentIdeas_AppUsers_AppUserId",
                table: "InvestmentIdeas",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentIdeas_AppUsers_AppUserId",
                table: "InvestmentIdeas");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentIdeas_AppUserId",
                table: "InvestmentIdeas");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "InvestmentIdeas");
        }
    }
}
