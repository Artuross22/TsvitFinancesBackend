using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvestmentIdea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Strategies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "Diversifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "InvestmentIdeaId",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvestmentIdeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Profit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentIdeas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_InvestmentIdeaId",
                table: "Assets",
                column: "InvestmentIdeaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_InvestmentIdeas_InvestmentIdeaId",
                table: "Assets",
                column: "InvestmentIdeaId",
                principalTable: "InvestmentIdeas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_InvestmentIdeas_InvestmentIdeaId",
                table: "Assets");

            migrationBuilder.DropTable(
                name: "InvestmentIdeas");

            migrationBuilder.DropIndex(
                name: "IX_Assets_InvestmentIdeaId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Diversifications");

            migrationBuilder.DropColumn(
                name: "InvestmentIdeaId",
                table: "Assets");
        }
    }
}
