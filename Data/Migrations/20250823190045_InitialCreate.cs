using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hedges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hedges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PositionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rules = table.Column<string>(type: "text", nullable: false),
                    AverageLevel = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasonalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartSeason = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndSeason = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasonalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BalanceFlows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceFlows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceFlows_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InvestmentIdeas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpectedReturn = table.Column<decimal>(type: "numeric", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentIdeas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentIdeas_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacroeconomicAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EconomicType = table.Column<int>(type: "integer", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroeconomicAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroeconomicAnalyses_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Futures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HedgeId = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HedgeId = table.Column<int>(type: "integer", nullable: false)
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
                name: "RiskManagements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BaseRiskPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    RiskToRewardRatio = table.Column<decimal>(type: "numeric", nullable: false),
                    HedgeId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskManagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiskManagements_Hedges_HedgeId",
                        column: x => x.HedgeId,
                        principalTable: "Hedges",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SectorHedge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HedgeId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PositionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinimumCorrectionPercent = table.Column<int>(type: "integer", nullable: false),
                    TimeFrame = table.Column<int>(type: "integer", nullable: false),
                    PositionManagementId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionRules_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionScalings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquityPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    PositionType = table.Column<int>(type: "integer", nullable: false),
                    PositionManagementId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionScalings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionScalings_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacroeconomicEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    MacroeconomicAnalysisId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroeconomicEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroeconomicEvents_MacroeconomicAnalyses_MacroeconomicAnal~",
                        column: x => x.MacroeconomicAnalysisId,
                        principalTable: "MacroeconomicAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diversifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    NichePercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Sector = table.Column<int>(type: "integer", nullable: false),
                    MinimumAssetsPerNiche = table.Column<int>(type: "integer", nullable: false),
                    RiskManagementId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diversifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diversifications_RiskManagements_RiskManagementId",
                        column: x => x.RiskManagementId,
                        principalTable: "RiskManagements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Strategies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    RiskManagementId = table.Column<int>(type: "integer", nullable: true),
                    PositionManagementId = table.Column<int>(type: "integer", nullable: true),
                    MacroeconomicEventId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strategies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Strategies_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Strategies_MacroeconomicEvents_MacroeconomicEventId",
                        column: x => x.MacroeconomicEventId,
                        principalTable: "MacroeconomicEvents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Strategies_PositionEntries_PositionManagementId",
                        column: x => x.PositionManagementId,
                        principalTable: "PositionEntries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Strategies_RiskManagements_RiskManagementId",
                        column: x => x.RiskManagementId,
                        principalTable: "RiskManagements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractId = table.Column<string>(type: "text", nullable: false),
                    Goal = table.Column<string>(type: "text", nullable: false),
                    Sector = table.Column<int>(type: "integer", nullable: false),
                    Term = table.Column<int>(type: "integer", nullable: false),
                    Market = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Ticker = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestOnCurrentDeposit = table.Column<decimal>(type: "numeric", nullable: false),
                    BoughtFor = table.Column<decimal>(type: "numeric", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SoldFor = table.Column<decimal>(type: "numeric", nullable: true),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    StrategyId = table.Column<int>(type: "integer", nullable: true),
                    InvestmentIdeaId = table.Column<int>(type: "integer", nullable: true),
                    SeasonalityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_InvestmentIdeas_InvestmentIdeaId",
                        column: x => x.InvestmentIdeaId,
                        principalTable: "InvestmentIdeas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Seasonalities_SeasonalityId",
                        column: x => x.SeasonalityId,
                        principalTable: "Seasonalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assets_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinanceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    StrategyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinanceData_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalTable: "Strategies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StrategyMacroeconomicEvents",
                columns: table => new
                {
                    StrategyId = table.Column<int>(type: "integer", nullable: false),
                    MacroeconomicEventId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategyMacroeconomicEvents", x => new { x.StrategyId, x.MacroeconomicEventId });
                    table.ForeignKey(
                        name: "FK_StrategyMacroeconomicEvents_MacroeconomicEvents_Macroeconom~",
                        column: x => x.MacroeconomicEventId,
                        principalTable: "MacroeconomicEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StrategyMacroeconomicEvents_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetHistory_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionEntryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    AssetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionEntryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionEntryNotes_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageLevel = table.Column<decimal>(type: "numeric", nullable: true),
                    AssetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseLevel_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<decimal>(type: "numeric", nullable: false),
                    AverageLevel = table.Column<decimal>(type: "numeric", nullable: true),
                    AssetId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesLevels_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CryptoMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketCap = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric", nullable: false),
                    YearHigh = table.Column<decimal>(type: "numeric", nullable: false),
                    YearLow = table.Column<decimal>(type: "numeric", nullable: false),
                    FinanceDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoMetrics_FinanceData_FinanceDataId",
                        column: x => x.FinanceDataId,
                        principalTable: "FinanceData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    PERatio = table.Column<decimal>(type: "numeric", nullable: false),
                    OperatingCashFlowPerShare = table.Column<decimal>(type: "numeric", nullable: false),
                    ROE = table.Column<decimal>(type: "numeric", nullable: false),
                    PBRatio = table.Column<decimal>(type: "numeric", nullable: false),
                    DividendYield = table.Column<decimal>(type: "numeric", nullable: false),
                    DebtToEquityRatio = table.Column<decimal>(type: "numeric", nullable: false),
                    EBIT = table.Column<decimal>(type: "numeric", nullable: false),
                    PSRatio = table.Column<decimal>(type: "numeric", nullable: false),
                    FreeCashFlowPerShare = table.Column<decimal>(type: "numeric", nullable: false),
                    ROA = table.Column<decimal>(type: "numeric", nullable: false),
                    NetProfitMargin = table.Column<decimal>(type: "numeric", nullable: false),
                    RevenueGrowth = table.Column<decimal>(type: "numeric", nullable: false),
                    DebtRatio = table.Column<decimal>(type: "numeric", nullable: false),
                    FreeCashFlow = table.Column<decimal>(type: "numeric", nullable: false),
                    NetIncome = table.Column<decimal>(type: "numeric", nullable: false),
                    SharesOutstanding = table.Column<decimal>(type: "numeric", nullable: false),
                    FinanceDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMetrics_FinanceData_FinanceDataId",
                        column: x => x.FinanceDataId,
                        principalTable: "FinanceData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PositionEntryNoteId = table.Column<int>(type: "integer", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charts_PositionEntryNotes_PositionEntryNoteId",
                        column: x => x.PositionEntryNoteId,
                        principalTable: "PositionEntryNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetHistory_AssetId",
                table: "AssetHistory",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AppUserId",
                table: "Assets",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_InvestmentIdeaId",
                table: "Assets",
                column: "InvestmentIdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_SeasonalityId",
                table: "Assets",
                column: "SeasonalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_StrategyId",
                table: "Assets",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFlows_AppUserId",
                table: "BalanceFlows",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_PositionEntryNoteId",
                table: "Charts",
                column: "PositionEntryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoMetrics_FinanceDataId",
                table: "CryptoMetrics",
                column: "FinanceDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diversifications_RiskManagementId",
                table: "Diversifications",
                column: "RiskManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_FinanceData_StrategyId",
                table: "FinanceData",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_Futures_HedgeId",
                table: "Futures",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentIdeas_AppUserId",
                table: "InvestmentIdeas",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MacroeconomicAnalyses_AppUserId",
                table: "MacroeconomicAnalyses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MacroeconomicEvents_MacroeconomicAnalysisId",
                table: "MacroeconomicEvents",
                column: "MacroeconomicAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_HedgeId",
                table: "Option",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionEntryNotes_AssetId",
                table: "PositionEntryNotes",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionRules_PositionManagementId",
                table: "PositionRules",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionScalings_PositionManagementId",
                table: "PositionScalings",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLevel_AssetId",
                table: "PurchaseLevel",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskManagements_HedgeId",
                table: "RiskManagements",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLevels_AssetId",
                table: "SalesLevels",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_SectorHedge_HedgeId",
                table: "SectorHedge",
                column: "HedgeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMetrics_FinanceDataId",
                table: "StockMetrics",
                column: "FinanceDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_AppUserId",
                table: "Strategies",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_MacroeconomicEventId",
                table: "Strategies",
                column: "MacroeconomicEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_PositionManagementId",
                table: "Strategies",
                column: "PositionManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_RiskManagementId",
                table: "Strategies",
                column: "RiskManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_StrategyMacroeconomicEvents_MacroeconomicEventId",
                table: "StrategyMacroeconomicEvents",
                column: "MacroeconomicEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssetHistory");

            migrationBuilder.DropTable(
                name: "BalanceFlows");

            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.DropTable(
                name: "CryptoMetrics");

            migrationBuilder.DropTable(
                name: "Diversifications");

            migrationBuilder.DropTable(
                name: "Futures");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "PositionRules");

            migrationBuilder.DropTable(
                name: "PositionScalings");

            migrationBuilder.DropTable(
                name: "PurchaseLevel");

            migrationBuilder.DropTable(
                name: "SalesLevels");

            migrationBuilder.DropTable(
                name: "SectorHedge");

            migrationBuilder.DropTable(
                name: "StockMetrics");

            migrationBuilder.DropTable(
                name: "StrategyMacroeconomicEvents");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PositionEntryNotes");

            migrationBuilder.DropTable(
                name: "FinanceData");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "InvestmentIdeas");

            migrationBuilder.DropTable(
                name: "Seasonalities");

            migrationBuilder.DropTable(
                name: "Strategies");

            migrationBuilder.DropTable(
                name: "MacroeconomicEvents");

            migrationBuilder.DropTable(
                name: "PositionEntries");

            migrationBuilder.DropTable(
                name: "RiskManagements");

            migrationBuilder.DropTable(
                name: "MacroeconomicAnalyses");

            migrationBuilder.DropTable(
                name: "Hedges");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
