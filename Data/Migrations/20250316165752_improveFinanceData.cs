using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class improveFinanceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinanceData_CryptoMetrics_CryptoMetricsId",
                table: "FinanceData");

            migrationBuilder.DropForeignKey(
                name: "FK_FinanceData_StockMetrics_StockMetricsId",
                table: "FinanceData");

            migrationBuilder.DropIndex(
                name: "IX_FinanceData_CryptoMetricsId",
                table: "FinanceData");

            migrationBuilder.DropIndex(
                name: "IX_FinanceData_StockMetricsId",
                table: "FinanceData");

            migrationBuilder.DropColumn(
                name: "CryptoMetricsId",
                table: "FinanceData");

            migrationBuilder.DropColumn(
                name: "StockMetricsId",
                table: "FinanceData");

            migrationBuilder.AddColumn<int>(
                name: "FinanceDataId",
                table: "StockMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinanceDataId",
                table: "CryptoMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockMetrics_FinanceDataId",
                table: "StockMetrics",
                column: "FinanceDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CryptoMetrics_FinanceDataId",
                table: "CryptoMetrics",
                column: "FinanceDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CryptoMetrics_FinanceData_FinanceDataId",
                table: "CryptoMetrics",
                column: "FinanceDataId",
                principalTable: "FinanceData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMetrics_FinanceData_FinanceDataId",
                table: "StockMetrics",
                column: "FinanceDataId",
                principalTable: "FinanceData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptoMetrics_FinanceData_FinanceDataId",
                table: "CryptoMetrics");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMetrics_FinanceData_FinanceDataId",
                table: "StockMetrics");

            migrationBuilder.DropIndex(
                name: "IX_StockMetrics_FinanceDataId",
                table: "StockMetrics");

            migrationBuilder.DropIndex(
                name: "IX_CryptoMetrics_FinanceDataId",
                table: "CryptoMetrics");

            migrationBuilder.DropColumn(
                name: "FinanceDataId",
                table: "StockMetrics");

            migrationBuilder.DropColumn(
                name: "FinanceDataId",
                table: "CryptoMetrics");

            migrationBuilder.AddColumn<int>(
                name: "CryptoMetricsId",
                table: "FinanceData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockMetricsId",
                table: "FinanceData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_CryptoMetricsId",
                table: "FinanceData",
                column: "CryptoMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_StockMetricsId",
                table: "FinanceData",
                column: "StockMetricsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceData_CryptoMetrics_CryptoMetricsId",
                table: "FinanceData",
                column: "CryptoMetricsId",
                principalTable: "CryptoMetrics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinanceData_StockMetrics_StockMetricsId",
                table: "FinanceData",
                column: "StockMetricsId",
                principalTable: "StockMetrics",
                principalColumn: "Id");
        }
    }
}
