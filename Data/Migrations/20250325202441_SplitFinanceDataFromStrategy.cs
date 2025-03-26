using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitFinanceDataFromStrategy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_FinanceData_FinanceDataId",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_FinanceDataId",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "FinanceDataId",
                table: "Strategies");

            migrationBuilder.AddColumn<int>(
                name: "StrategyId",
                table: "FinanceData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_StrategyId",
                table: "FinanceData",
                column: "StrategyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceData_Strategies_StrategyId",
                table: "FinanceData",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinanceData_Strategies_StrategyId",
                table: "FinanceData");

            migrationBuilder.DropIndex(
                name: "IX_FinanceData_StrategyId",
                table: "FinanceData");

            migrationBuilder.DropColumn(
                name: "StrategyId",
                table: "FinanceData");

            migrationBuilder.AddColumn<int>(
                name: "FinanceDataId",
                table: "Strategies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_FinanceDataId",
                table: "Strategies",
                column: "FinanceDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_FinanceData_FinanceDataId",
                table: "Strategies",
                column: "FinanceDataId",
                principalTable: "FinanceData",
                principalColumn: "Id");
        }
    }
}
