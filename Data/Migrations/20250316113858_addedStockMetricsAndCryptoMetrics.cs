using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addedStockMetricsAndCryptoMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinanceDataId",
                table: "Strategies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CryptoMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarketCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearHigh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearLow = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PERatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperatingCashFlowPerShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PBRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DividendYield = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtToEquityRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EBIT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PSRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreeCashFlowPerShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROA = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetProfitMargin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RevenueGrowth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreeCashFlow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SharesOutstanding = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinanceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CryptoMetricsId = table.Column<int>(type: "int", nullable: true),
                    StockMetricsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceData_CryptoMetrics_CryptoMetricsId",
                        column: x => x.CryptoMetricsId,
                        principalTable: "CryptoMetrics",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinanceData_StockMetrics_StockMetricsId",
                        column: x => x.StockMetricsId,
                        principalTable: "StockMetrics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_FinanceDataId",
                table: "Strategies",
                column: "FinanceDataId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_CryptoMetricsId",
                table: "FinanceData",
                column: "CryptoMetricsId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_StockMetricsId",
                table: "FinanceData",
                column: "StockMetricsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_FinanceData_FinanceDataId",
                table: "Strategies",
                column: "FinanceDataId",
                principalTable: "FinanceData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_FinanceData_FinanceDataId",
                table: "Strategies");

            migrationBuilder.DropTable(
                name: "FinanceData");

            migrationBuilder.DropTable(
                name: "CryptoMetrics");

            migrationBuilder.DropTable(
                name: "StockMetrics");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_FinanceDataId",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "FinanceDataId",
                table: "Strategies");
        }
    }
}
