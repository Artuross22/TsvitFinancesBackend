using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHedgeandDiversInRiskManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskManagements_Hedges_HedgeId",
                table: "RiskManagements");

            migrationBuilder.AlterColumn<int>(
                name: "HedgeId",
                table: "RiskManagements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DiversificationId",
                table: "RiskManagements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements",
                column: "DiversificationId",
                principalTable: "Diversifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskManagements_Hedges_HedgeId",
                table: "RiskManagements",
                column: "HedgeId",
                principalTable: "Hedges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiskManagements_Hedges_HedgeId",
                table: "RiskManagements");

            migrationBuilder.AlterColumn<int>(
                name: "HedgeId",
                table: "RiskManagements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiversificationId",
                table: "RiskManagements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements",
                column: "DiversificationId",
                principalTable: "Diversifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiskManagements_Hedges_HedgeId",
                table: "RiskManagements",
                column: "HedgeId",
                principalTable: "Hedges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
