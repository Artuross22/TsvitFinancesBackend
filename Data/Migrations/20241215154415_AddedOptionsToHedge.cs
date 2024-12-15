using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOptionsToHedge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Futures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HedgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Futures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Futures_Hedges_HedgeId",
                        column: x => x.HedgeId,
                        principalTable: "Hedges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HedgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_Hedges_HedgeId",
                        column: x => x.HedgeId,
                        principalTable: "Hedges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectorHedge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HedgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectorHedge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectorHedge_Hedges_HedgeId",
                        column: x => x.HedgeId,
                        principalTable: "Hedges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Futures_HedgeId",
                table: "Futures",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_HedgeId",
                table: "Option",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorHedge_HedgeId",
                table: "SectorHedge",
                column: "HedgeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Futures");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "SectorHedge");
        }
    }
}
