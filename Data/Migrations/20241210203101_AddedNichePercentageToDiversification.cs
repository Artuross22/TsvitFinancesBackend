using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNichePercentageToDiversification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements");

            migrationBuilder.DropIndex(
                name: "IX_RiskManagements_DiversificationId",
                table: "RiskManagements");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "RiskManagements");

            migrationBuilder.DropColumn(
                name: "DiversificationId",
                table: "RiskManagements");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Diversifications");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Strategies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "NichePercentage",
                table: "Diversifications",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RiskManagementId",
                table: "Diversifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sector",
                table: "Diversifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Diversifications_RiskManagementId",
                table: "Diversifications",
                column: "RiskManagementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diversifications_RiskManagements_RiskManagementId",
                table: "Diversifications",
                column: "RiskManagementId",
                principalTable: "RiskManagements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diversifications_RiskManagements_RiskManagementId",
                table: "Diversifications");

            migrationBuilder.DropIndex(
                name: "IX_Diversifications_RiskManagementId",
                table: "Diversifications");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Strategies");

            migrationBuilder.DropColumn(
                name: "NichePercentage",
                table: "Diversifications");

            migrationBuilder.DropColumn(
                name: "RiskManagementId",
                table: "Diversifications");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Diversifications");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "RiskManagements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiversificationId",
                table: "RiskManagements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Diversifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RiskManagements_DiversificationId",
                table: "RiskManagements",
                column: "DiversificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskManagements_Diversifications_DiversificationId",
                table: "RiskManagements",
                column: "DiversificationId",
                principalTable: "Diversifications",
                principalColumn: "Id");
        }
    }
}
