using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScalingOutAndScalingIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScalingIn",
                table: "PositionEntries");

            migrationBuilder.DropColumn(
                name: "ScalingOut",
                table: "PositionEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ScalingIn",
                table: "PositionEntries",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ScalingOut",
                table: "PositionEntries",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
